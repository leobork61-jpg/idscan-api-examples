using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdScan.Example.Models;

public sealed class ScanResult
{
    [JsonPropertyName("VerificationRequestId")]
    public Guid VerificationRequestId { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("EmailAddress")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("PhoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("PreferredLanguage")]
    public string PreferredLanguage { get; set; } = string.Empty;

    [JsonPropertyName("Status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("RequestType")]
    public string RequestType { get; set; } = string.Empty;

    [JsonPropertyName("DeliveryMode")]
    public string DeliveryMode { get; set; } = string.Empty;

    [JsonPropertyName("DeliveryChannels")]
    public string DeliveryChannels { get; set; } = string.Empty;

    [JsonPropertyName("ResultJson")]
    public string? ResultJson { get; set; }

    [JsonPropertyName("CreatedDateTimeUtc")]
    public DateTime? CreatedDateTimeUtc { get; set; }

    [JsonPropertyName("InvitationSentDateTimeUtc")]
    public DateTime? InvitationSentDateTimeUtc { get; set; }

    [JsonPropertyName("CompletedDateTimeUtc")]
    public DateTime? CompletedDateTimeUtc { get; set; }

    [JsonPropertyName("HasResult")]
    public bool HasResult { get; set; }

    [JsonPropertyName("IsSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("NfcReadSuccess")]
    public bool NfcReadSuccess { get; set; }

    [JsonPropertyName("ChipAuthenticationSuccess")]
    public bool ChipAuthenticationSuccess { get; set; }

    [JsonPropertyName("Summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("DocumentType")]
    public string DocumentType { get; set; } = string.Empty;

   

    public ParsedResultJson? ParseResultJson()
    {
        if (string.IsNullOrWhiteSpace(ResultJson))
        {
            return null;
        }

        return JsonSerializer.Deserialize<ParsedResultJson>(ResultJson);
    }
}

public sealed class ParsedResultJson
{
    [JsonPropertyName("fields")]
    public ResultFields? Fields { get; set; }

    [JsonPropertyName("checks")]
    public ResultChecks? Checks { get; set; }

    [JsonPropertyName("compliance")]
    public ComplianceResult? Compliance { get; set; }
}

public sealed class ResultFields
{
    [JsonPropertyName("given_names")]
    public string? GivenNames { get; set; }

    [JsonPropertyName("surname")]
    public string? Surname { get; set; }

    [JsonPropertyName("document_number")]
    public string? DocumentNumber { get; set; }

    [JsonPropertyName("nationality")]
    public string? Nationality { get; set; }

    [JsonPropertyName("country_of_issue")]
    public string? CountryOfIssue { get; set; }

    [JsonPropertyName("document_type")]
    public string? DocumentType { get; set; }

    [JsonPropertyName("date_of_birth")]
    public string? DateOfBirth { get; set; }

    [JsonPropertyName("date_of_expiry")]
    public string? DateOfExpiry { get; set; }
}

public sealed class ResultChecks
{
    [JsonPropertyName("chip_authentication_success")]
    public bool? ChipAuthenticationSuccess { get; set; }

    [JsonPropertyName("nfc_read_success")]
    public bool? NfcReadSuccess { get; set; }

    [JsonPropertyName("passive_authentication_success")]
    public bool? PassiveAuthenticationSuccess { get; set; }

    [JsonPropertyName("selfie_check_success")]
    public bool? SelfieCheckSuccess { get; set; }
}
