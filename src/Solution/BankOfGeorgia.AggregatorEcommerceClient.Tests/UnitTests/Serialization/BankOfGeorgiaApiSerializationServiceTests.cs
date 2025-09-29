namespace BankOfGeorgia.AggregatorEcommerceClient.Tests;

public class BankOfGeorgiaApiSerializationServiceTests
{
    [Fact]
    public void SerializeAndDeserialize_Test()
    {
        // Arrange
        BankOfGeorgiaApiSerializationService sut = new();
        SubmitOrderAggregatorRequest request = new()
        {
            CallbackUrl = "",
            Capture = CaptureType.Automatic,
            PaymentMethod = [PaymentMethod.Card],
            PurchaseUnits = new PurchaseUnits()
            {
                Basket = [],
                TotalAmount = 12
            }
        };

        string serialized = sut.Serialize(request);

        // Act
        var deserialized = sut.Deserialize<SubmitOrderAggregatorRequest>(serialized);

        // Assert
        Assert.Multiple(
            () => Assert.Equivalent(request, deserialized),
            () => Assert.Contains("capture\":\"automatic\",", serialized, StringComparison.Ordinal),
            () => Assert.DoesNotContain("capture\":\"auTomatic\",", serialized, StringComparison.Ordinal),
            () => Assert.True(deserialized!.Capture == CaptureType.Automatic)
        );
    }

    [Fact]
    public void Deserialize_Test()
    {
        // Arrange
        BankOfGeorgiaApiSerializationService sut = new();
        string serialized = """
            {
            	"order_id": "1fcf4604-a135-4739-9c06-1064bc32f7ea",
            	"external_order_id": "0e632f8b-4d6f-493e-9152-70a0aef73975",
            	"client": {
            		"id": "0001",
            		"brand_ka": "Brand LTD",
            		"brand_en": "branch LTD",
            		"url": "https://example.com"
            	},
            	"create_date": "2025-09-29T16:57:54.595205",
            	"zoned_create_date": "2025-09-29T12:57:54.595205Z",
            	"expire_date": "2025-09-29T17:12:54.595205",
            	"zoned_expire_date": "2025-09-29T13:12:54.595205Z",
            	"order_status": {
            		"key": "created",
            		"value": "Created"
            	},
            	"buyer": null,
            	"redirect_links": {
            		"success": "https://localhost/success",
            		"fail": "https://localhost/fail"
            	},
            	"payment_detail": {
            		"transfer_method": {
            			"key": "",
            			"value": ""
            		},
            		"transaction_id": null,
            		"payer_identifier": null,
            		"payment_option": "direct_debit",
            		"card_type": null,
            		"card_expiry_date": null,
            		"request_account_tag": null,
            		"transfer_account_tag": null,
            		"saved_card_type": null,
            		"parent_order_id": null,
            		"code": null,
            		"code_description": null,
            		"pg_trx_id": null,
            		"auth_code": null
            	},
            	"actions": null,
            	"disputes": null,
            	"split": null,
            	"discount": null,
            	"lang": "ka",
            	"reject_reason": null,
            	"industry": "ecommerce",
            	"capture": "automatic",
            	"purchase_units": {
            		"request_amount": "5.3",
            		"transfer_amount": null,
            		"refund_amount": null,
            		"currency_code": "GEL",
            		"items": [
            			{
            				"external_item_id": "1",
            				"unit_price": "1.22",
            				"description": null,
            				"quantity": "1",
            				"unit_discount_price": "0.0",
            				"package_code": null,
            				"total_price": null,
            				"vat": null,
            				"vat_percent": null,
            				"tin": null,
            				"pinfl": null,
            				"product_discount_id": null
            			},
            			{
            				"external_item_id": "1",
            				"unit_price": "1.04",
            				"description": null,
            				"quantity": "2",
            				"unit_discount_price": "0.0",
            				"package_code": null,
            				"total_price": null,
            				"vat": null,
            				"vat_percent": null,
            				"tin": null,
            				"pinfl": null,
            				"product_discount_id": null
            			}
            		]
            	}
            }
            """;

        // Act
        var result = sut.Deserialize<GetOrderDetailsAggregatorResponse>(serialized);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal("1fcf4604-a135-4739-9c06-1064bc32f7ea", result!.OrderId),
            () => Assert.Equal("0e632f8b-4d6f-493e-9152-70a0aef73975", result!.ExternalOrderId),
            () => Assert.Equal(OrderStatusType.Created, result!.OrderStatus?.Key),
            () => Assert.Equal("Created", result!.OrderStatus?.Value),
            () => Assert.Equal(UiLanguage.Georgian, result!.Lang),
            () => Assert.Equal(CaptureType.Automatic, result!.Capture),
            () => Assert.Equal("direct_debit", result!.PaymentDetail?.PaymentOption),
            () => Assert.Equal(PaymentMethod.Unknown, result!.PaymentDetail?.TransferMethod?.Key),
            () => Assert.Equal("5.3", result!.PurchaseUnits?.RequestAmount?.ToString()),
            () => Assert.Equal("GEL", result!.PurchaseUnits?.CurrencyCode),
            () => Assert.Equal(2, result!.PurchaseUnits?.Items?.Count()),
            () => Assert.Equal("https://localhost/success", result!.RedirectLinks?.Success),
            () => Assert.Equal("https://localhost/fail", result!.RedirectLinks?.Fail)
        );
    }
}
