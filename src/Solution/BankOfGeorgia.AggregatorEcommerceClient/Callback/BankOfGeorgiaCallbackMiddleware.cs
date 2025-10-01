using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class BankOfGeorgiaCallbackMiddleware(
    RequestDelegate next,
    ILogger<BankOfGeorgiaCallbackMiddleware> logger,
    ICallbackRequestVerificationService verifier,
    IBankOfGeorgiaApiSerializationService serializer,
    string callbackPath,
    IBankOfGeorgiaAggregatorCallbackHandler callbackHandler)
{
    private readonly string _callbackPath = ValidateAndNormalizePath(callbackPath);

    public async Task InvokeAsync(HttpContext context)
    {
        if (!HttpMethods.IsPost(context.Request.Method))
        {
            await next(context);
            return;
        }

        if (context.Request.Path.Equals(_callbackPath, StringComparison.OrdinalIgnoreCase) is false)
        {
            await next(context);
            return;
        }

        try
        {
            await HandleCallbackAsync(context);
        }
        catch (CallbackValidationException ex)
        {
            logger.LogError(
                ex,
                "Callback validation failed. RequestBody: '{RequestBody}'; Signature: '{Signature}'",
                ex.RequestBody,
                ex.Signature);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        context.Response.StatusCode = StatusCodes.Status200OK;
    }

    private static string ValidateAndNormalizePath(string callbackPath)
    {
        if (string.IsNullOrWhiteSpace(callbackPath))
        {
            throw new ArgumentException("Callback path cannot be null or empty.", nameof(callbackPath));
        }

        if (!callbackPath.StartsWith('/'))
        {
            throw new ArgumentException("Callback path must start with '/'.", nameof(callbackPath));
        }

        if (callbackPath.Contains('?') || callbackPath.Contains('#'))
        {
            throw new ArgumentException("Callback path cannot contain query string or fragment.", nameof(callbackPath));
        }

        if (callbackPath.Contains("//"))
        {
            throw new ArgumentException("Callback path cannot contain double slashes.", nameof(callbackPath));
        }

        try
        {
            Uri uri = new(callbackPath, UriKind.Relative);
            if (uri.IsAbsoluteUri)
            {
                throw new ArgumentException("Callback path must be a relative path.", nameof(callbackPath));
            }
        }
        catch (UriFormatException ex)
        {
            throw new ArgumentException($"Callback path is not a valid URI: {ex.Message}", nameof(callbackPath), ex);
        }

        if (callbackPath.EndsWith('/') && callbackPath.Length > 1)
        {
            return callbackPath.TrimEnd('/');
        }

        return callbackPath;
    }

    private string ReadSignature(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Callback-Signature", out var signatureValues))
        {
            return signatureValues.FirstOrDefault() ?? string.Empty;
        }

        throw new CallbackValidationException("Callback-Signature header is missing or empty");
    }

    private async Task<string> ReadBody(HttpContext context)
    {
        context.Request.EnableBuffering();
        using StreamReader reader = new(context.Request.Body);
        string bodyMessage = await reader.ReadToEndAsync();

        if (string.IsNullOrEmpty(bodyMessage))
        {
            throw new CallbackValidationException("Request body is empty");
        }

        return bodyMessage;
    }

    private CallbackRequest DeserializeBody(string body)
    {
        CallbackRequest? request;
        try
        {
            request = serializer.Deserialize<CallbackRequest>(body);
        }
        catch (Exception ex)
        {
            throw new CallbackValidationException("Failed to deserialize callback request", body, ex);
        }

        if (request is null)
        {
            throw new CallbackValidationException("Callback request deserialized into null", body);
        }

        return request;
    }

    private async Task HandleCallbackAsync(HttpContext context)
    {
        string signature = ReadSignature(context);
        string body = await ReadBody(context);

        verifier.ValidateSignature(body, signature);

        CallbackRequest request = DeserializeBody(body);

        await callbackHandler.Handle(request);
    }
}
