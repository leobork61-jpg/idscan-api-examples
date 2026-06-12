using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdScan.Example.Models
{
  public sealed class ComplianceResult
  {
    [JsonPropertyName("requested")]
    public bool? Requested { get; set; }

    [JsonPropertyName("provider")]
    public string? Provider { get; set; }

    [JsonPropertyName("match_found")]
    public bool? MatchFound { get; set; }

    [JsonPropertyName("pep_match")]
    public bool? PepMatch { get; set; }

    [JsonPropertyName("sanctions_match")]
    public bool? SanctionsMatch { get; set; }

    [JsonPropertyName("rca_match")]
    public bool? RcaMatch { get; set; }

    [JsonPropertyName("highest_score")]
    public decimal? HighestScore { get; set; }

    [JsonPropertyName("matches")]
    public List<ComplianceMatch> Matches { get; set; } = new();
  }
}
