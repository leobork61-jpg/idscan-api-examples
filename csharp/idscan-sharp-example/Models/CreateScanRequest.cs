using System.Text.Json.Serialization;

namespace IdScan.Example.Models;

public sealed class CreateScanRequest
{
    [JsonPropertyName("templateCode")]
    public string? TemplateCode { get; set; }

    [JsonPropertyName("requestedFieldsJson")]
    public string? RequestedFieldsJson { get; set; }

    [JsonPropertyName("languageCode")]
    public string LanguageCode { get; set; } = "nl";

    [JsonPropertyName("externalReference")]
    public string ExternalReference { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("requestType")]
    public string RequestType { get; set; } = "identity_check";

    [JsonPropertyName("expiryInHours")]
    public int ExpiryInHours { get; set; } = 48;

    [JsonPropertyName("delivery")]
    public ScanRequestDelivery Delivery { get; set; } = new();
   
}

public sealed class ScanRequestDelivery
{
    [JsonPropertyName("mode")]
    public string Mode { get; set; } = "platform";

    [JsonPropertyName("channels")]
    public List<string> Channels { get; set; } = new() { "sms", "email" };
}
