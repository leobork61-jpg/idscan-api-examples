using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IdScan.Example.Models
{
  public sealed class ComplianceMatch
  {
    [JsonPropertyName("entity_id")]
    public string? EntityId { get; set; }

    [JsonPropertyName("caption")]
    public string? Caption { get; set; }

    [JsonPropertyName("score")]
    public decimal? Score { get; set; }

    [JsonPropertyName("pep")]
    public bool? Pep { get; set; }

    [JsonPropertyName("sanction")]
    public bool? Sanction { get; set; }

    [JsonPropertyName("rca")]
    public bool? Rca { get; set; }

    [JsonPropertyName("topics")]
    public List<string> Topics { get; set; } = new();
  }
}
