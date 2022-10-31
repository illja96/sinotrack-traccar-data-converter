using System.Text.Json.Serialization;

namespace SinoTrackTraccarDataConverter.CLI.ResponseModels;

internal class LoginResponseModel
{
    [JsonPropertyName("m_arrField")]
    public string[] RecordFields { get; set; }

    [JsonPropertyName("m_arrRecord")]
    public string[][] Records { get; set; }

    [JsonPropertyName("m_isResultOk")]
    public int IsResultOk { get; set; }
}
