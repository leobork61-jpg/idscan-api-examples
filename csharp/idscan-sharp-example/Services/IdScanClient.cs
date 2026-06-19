using System.Net.Http.Json;
using IdScan.Example.Models;

namespace IdScan.Example.Services;

public sealed class IdScanClient
{
  private readonly HttpClient _httpClient;

  public IdScanClient(HttpClient httpClient)
  {
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
  }

  public async Task<CreateScanResponse> CreateScanRequestAsync(
      CreateScanRequest request,
      CancellationToken cancellationToken = default)
  {
    ArgumentNullException.ThrowIfNull(request);

    var response = await _httpClient.PostAsJsonAsync(
        "scan-requests",
        request,
        cancellationToken);

    await EnsureSuccessAsync(response, cancellationToken);

    var result = await response.Content.ReadFromJsonAsync<CreateScanResponse>(
        cancellationToken: cancellationToken);

    return result ?? throw new InvalidOperationException("The API returned an empty response.");
  }

  public async Task<ScanResult> GetScanResultAsync(
      string requestId,
      CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(requestId))
    {
      throw new ArgumentException("Request ID is required.", nameof(requestId));
    }

    var response = await _httpClient.GetAsync(
        $"scan-requests/{Uri.EscapeDataString(requestId)}/details",
        cancellationToken);

    await EnsureSuccessAsync(response, cancellationToken);

    var result = await response.Content.ReadFromJsonAsync<ScanResult>(
        cancellationToken: cancellationToken);

    return result ?? throw new InvalidOperationException("The API returned an empty response.");
  }

  public async Task<byte[]> GetVerificationReportPdfAsync(
    string requestId,
    CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(requestId))
    {
      throw new ArgumentException("Request ID is required.", nameof(requestId));
    }

    var response = await _httpClient.GetAsync(
        $"scan-requests/{Uri.EscapeDataString(requestId)}/report.pdf",
        cancellationToken);

    await EnsureSuccessAsync(response, cancellationToken);

    return await response.Content.ReadAsByteArrayAsync(cancellationToken);
  }

  private static async Task EnsureSuccessAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
  {
    if (response.IsSuccessStatusCode)
    {
      return;
    }

    var body = await response.Content.ReadAsStringAsync(cancellationToken);

    throw new HttpRequestException(
        $"ID Scan API request failed with status {(int)response.StatusCode} {response.ReasonPhrase}. Response: {body}");
  }
}
