using IdScan.Example.Models;
using IdScan.Example.Services;
using System.Runtime.CompilerServices;

var baseUrl = Environment.GetEnvironmentVariable("IDSCAN_API_BASE_URL")
              ?? "https://api.id-scan.app/";


// Set IDSCAN_API_KEY in your environment instead.
var apiKey = Environment.GetEnvironmentVariable("IDSCAN_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
  Console.WriteLine("Missing API key.");
  Console.WriteLine("Get your API key from the ID Scan Portal.");
  Console.WriteLine("Navigate to Settings > API & Integrations.");
  Console.WriteLine("Then set the IDSCAN_API_KEY environment variable before running this example.");
  return;
}

using var httpClient = new HttpClient
{
  BaseAddress = new Uri(baseUrl)
};

httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);

var client = new IdScanClient(httpClient);

// TemplateCode is optional.
// When provided, the requested fields and verification checks are determined
// by the template configuration in the ID Scan Portal.
// Without a template, requested fields and checks can be supplied directly
// in the API request.
var useTemplateCode = true;

CreateScanRequest request = useTemplateCode
? CreateRequestWithTemplateCode()
: CreateRequestWithRequestedFieldsJson();

Console.WriteLine("Creating identity verification request...");

try
{
  var created = await client.CreateScanRequestAsync(request);

  Console.WriteLine();
  Console.WriteLine("Request created successfully.");
  Console.WriteLine($"ScanRequestId: {created.ScanRequestId}");
  Console.WriteLine($"Status: {created.Status}");
  Console.WriteLine($"LanguageCode: {created.LanguageCode}");
  Console.WriteLine($"RequestKey: {created.RequestKey}");
  Console.WriteLine($"ExpiresAtUtc: {created.ExpiresAtUtc:O}");
  Console.WriteLine($"DeepLinkUrl: {created.DeepLinkUrl}");
  Console.WriteLine($"WebUrl: {created.WebUrl}");

  Console.WriteLine();
  Console.WriteLine("Open the verification link and complete the ID scan.");
  Console.WriteLine("Press Enter when the verification is completed.");
  Console.ReadLine();

  Console.WriteLine("Retrieving current request status...");

  var result = await client.GetScanResultAsync(created.ScanRequestId.ToString());


  var parsedResultJson = result.ParseResultJson();

  Console.WriteLine();
  Console.WriteLine("Verification result");
  Console.WriteLine($"VerificationRequestId: {result.VerificationRequestId}");
  Console.WriteLine($"Name: {result.Name}");
  Console.WriteLine($"Status: {result.Status}");
  Console.WriteLine($"RequestType: {result.RequestType}");
  Console.WriteLine($"Summary: {result.Summary}");
  Console.WriteLine($"DocumentType: {result.DocumentType}");
  Console.WriteLine($"HasResult: {result.HasResult}");
  Console.WriteLine($"IsSuccess: {result.IsSuccess}");
  Console.WriteLine($"NfcReadSuccess: {result.NfcReadSuccess}");
  Console.WriteLine($"ChipAuthenticationSuccess: {result.ChipAuthenticationSuccess}");
  Console.WriteLine($"CompletedDateTimeUtc: {result.CompletedDateTimeUtc:O}");

  if (parsedResultJson?.Fields is not null)
  {
    Console.WriteLine();
    Console.WriteLine("Returned fields");
    Console.WriteLine($"Given names: {parsedResultJson.Fields.GivenNames}");
    Console.WriteLine($"Surname: {parsedResultJson.Fields.Surname}");
    Console.WriteLine($"Document number: {parsedResultJson.Fields.DocumentNumber}");
    Console.WriteLine($"Nationality: {parsedResultJson.Fields.Nationality}");
    Console.WriteLine($"Country of issue: {parsedResultJson.Fields.CountryOfIssue}");
    Console.WriteLine($"Date of birth: {parsedResultJson.Fields.DateOfBirth}");
    Console.WriteLine($"Date of expiry: {parsedResultJson.Fields.DateOfExpiry}");
  }

  if (parsedResultJson?.Checks is not null)
  {
    Console.WriteLine();
    Console.WriteLine("Checks from ResultJson");
    Console.WriteLine($"Chip authentication success: {parsedResultJson.Checks.ChipAuthenticationSuccess}");
    Console.WriteLine($"NFC read success: {parsedResultJson.Checks.NfcReadSuccess}");
    Console.WriteLine($"Passive authentication success: {parsedResultJson.Checks.PassiveAuthenticationSuccess}");
    Console.WriteLine($"Selfie check success: {parsedResultJson.Checks.SelfieCheckSuccess}");
  }

  if (parsedResultJson?.Compliance is not null)
  {
    Console.WriteLine();
    Console.WriteLine("Compliance screening");
    Console.WriteLine($"Provider: {parsedResultJson.Compliance.Provider}");
    Console.WriteLine($"Finding found: {parsedResultJson.Compliance.MatchFound}");
    Console.WriteLine($"PEP match: {parsedResultJson.Compliance.PepMatch}");
    Console.WriteLine($"Sanctions match: {parsedResultJson.Compliance.SanctionsMatch}");
    Console.WriteLine($"RCA match: {parsedResultJson.Compliance.RcaMatch}");
    Console.WriteLine($"Highest score: {parsedResultJson.Compliance.HighestScore}");

    if (parsedResultJson.Compliance.Matches.Any())
    {
      Console.WriteLine();
      Console.WriteLine("Compliance findings");
      foreach (var match in parsedResultJson.Compliance.Matches)
      {
        Console.WriteLine($"- {match.Caption} (score: {match.Score:0.00})");
      }
    }
  }

  Console.WriteLine();
  Console.WriteLine("Downloading signed verification report...");

  var reportPdf = await client.GetVerificationReportPdfAsync(
      created.ScanRequestId.ToString());

  var reportFileName = $"idscan-report-{created.ScanRequestId}.pdf";

  await File.WriteAllBytesAsync(reportFileName, reportPdf);

  Console.WriteLine($"Report saved to: {Path.GetFullPath(reportFileName)}");
  Console.WriteLine("The PDF report can be verified in Adobe Reader.");
}
catch (HttpRequestException ex)
{
  Console.WriteLine("The API request failed.");
  Console.WriteLine(ex.Message);
}
catch (Exception ex)
{
  Console.WriteLine("Unexpected error.");
  Console.WriteLine(ex.Message);
}

static CreateScanRequest CreateRequestWithTemplateCode()
{
  return new CreateScanRequest
  {
    TemplateCode = "default",
    LanguageCode = "en",
    ExternalReference = string.Empty,
    FirstName = "John",
    LastName = "Doe",
    EmailAddress = "johndoe@example.com",
    PhoneNumber = "31600000000",
    RequestType = "identity_check",
    ExpiryInHours = 48,
    Delivery = new ScanRequestDelivery
    {
      Mode = "platform",
      Channels = new List<string> { "sms", "email" }
    }
  };

}
static CreateScanRequest CreateRequestWithRequestedFieldsJson()
{
  return new CreateScanRequest
  {
    LanguageCode = "nl",
    ExternalReference = "string",
    FirstName = "John",
    LastName = "Doe",
    EmailAddress = "johndoe@example.com",
    PhoneNumber = "31612345678",
    RequestType = "identity_check",
    ExpiryInHours = 48,

    Delivery = new ScanRequestDelivery
    {
      Mode = "platform",
      Channels = new List<string>
        {
           // "sms",
            "email"
        }
    },

    RequestedFieldsJson = """
    {
      "fields": {
        "given_names": { "requested": true },
        "surname": { "requested": true },
        "document_number": {
          "requested": true,
          "masking": "last4_visible"
        },
        "country_of_issue": { "requested": true },
        "photo": { "requested": true },
        "date_of_expiry": { "requested": true },
        "date_of_birth": { "requested": true },
        "nationality": { "requested": true },
        "mrz_masked_image": { "requested": true }
      },
      "checks": {
        "document_not_expired": true,
        "nfc_read_success": true,
        "chip_authentication_success": true,
        "selfie_check": true,
        "minimum_age": 18,
        "compliance_screening": true
      }
    }
    """
  };
}

