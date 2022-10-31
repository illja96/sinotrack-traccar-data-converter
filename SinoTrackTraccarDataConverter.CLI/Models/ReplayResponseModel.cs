using System.Text.Json.Serialization;

namespace SinoTrackTraccarDataConverter.CLI.Models;

internal class ReplayResponseModel
{
    [JsonPropertyName("m_arrField")]
    public string[] RecordFields { get; set; }

    [JsonPropertyName("m_arrRecord")]
    public object[] Records { get; set; }

    [JsonPropertyName("m_nCount")]
    public int RecordsCount { get; set; }

    [JsonPropertyName("m_nTotal")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("m_isResultOk")]
    public int IsResultOk { get; set; }

    [JsonPropertyName("m_nAbsolutePage")]
    public int Page { get; set; }

    [JsonPropertyName("m_nPageCount")]
    public int TotalPages { get; set; }

    [JsonPropertyName("m_nPageSize")]
    public int MaxPageSize { get; set; }
}
