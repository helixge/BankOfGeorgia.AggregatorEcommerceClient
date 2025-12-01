using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace BankOfGeorgia.AggregatorEcommerceClient;

public class BankOfGeorgiaCallbackMiddleware(
    RequestDelegate next,
    ILogger<BankOfGeorgiaCallbackMiddleware> logger,
    ICallbackRequestVerificationService verifier,
    IBankOfGeorgiaApiSerializationService serializer,
    string callbackPath)
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

    private byte[] ReadSignature(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Callback-Signature", out StringValues signatureValues))
        {
            throw new CallbackValidationException("Callback-Signature header is missing or empty");
        }

        string? signatureString = signatureValues.FirstOrDefault();
        if (string.IsNullOrEmpty(signatureString))
        {
            throw new CallbackValidationException("Callback-Signature header is empty");
        }

        try
        {
            return Convert.FromBase64String(signatureString);
        }
        catch (FormatException ex)
        {
            throw new CallbackValidationException("Callback-Signature header is not valid Base64", string.Empty, signatureString, ex);
        }
    }

    private async Task<byte[]> ReadBody(HttpContext context)
    {
        context.Request.EnableBuffering();
        using MemoryStream ms = new();
        await context.Request.Body.CopyToAsync(ms);
        byte[] bodyBytes = ms.ToArray();

        if (bodyBytes.Length == 0)
        {
            throw new CallbackValidationException("Request body is empty");
        }

        return bodyBytes;
    }

    private CallbackRequest DeserializeBody(Span<byte> body)
    {
        CallbackRequest? request;
        try
        {
            request = serializer.Deserialize<CallbackRequest>(body);
        }
        catch (Exception ex)
        {
            string bodyString = Encoding.UTF8.GetString(body);
            throw new CallbackValidationException("Failed to deserialize callback request", bodyString, ex);
        }

        if (request is null)
        {
            string bodyString = Encoding.UTF8.GetString(body);
            throw new CallbackValidationException("Callback request deserialized into null", bodyString);
        }

        return request;
    }

    private async Task HandleCallbackAsync(HttpContext context)
    {
        byte[] signature = ReadSignature(context);
        byte[] body = await ReadBody(context);

        verifier.ValidateSignature(body, signature);

        CallbackRequest request = DeserializeBody(body);

        var callbackHandler = context.RequestServices.GetRequiredService<IBankOfGeorgiaAggregatorCallbackHandler>();
        await callbackHandler.Handle(request);
    }
}
