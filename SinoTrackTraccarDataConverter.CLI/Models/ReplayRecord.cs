using System.Text.Json.Serialization;

namespace SinoTrackTraccarDataConverter.CLI.Models;

internal class ReplayRecord
{
    [JsonPropertyName("nID")]
    public int RecordId { get; set; }

    [JsonPropertyName("strTEID")]
    public string TEID { get; set; }

    [JsonPropertyName("nTime")]
    public long UnixTimeSeconds { get; set; }

    [JsonIgnore]
    public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(UnixTimeSeconds);

    [JsonPropertyName("dbLon")]
    public decimal Longitude { get; set; }

    [JsonPropertyName("dbLat")]
    public decimal Latitude { get; set; }

    [JsonPropertyName("nDirection")]
    public int Direction { get; set; }

    [JsonPropertyName("nSpeed")]
    public int Speed { get; set; }

    [JsonPropertyName("nGSMSignal")]
    public int GSMSignal { get; set; }

    [JsonPropertyName("nGPSSignal")]
    public int GPSSignal { get; set; }

    [JsonPropertyName("nFuel")]
    public int Fuel { get; set; }

    [JsonPropertyName("nMileage")]
    public int Mileage { get; set; }

    [JsonPropertyName("nTemp")]
    public int Temp { get; set; }

    [JsonPropertyName("nCarState")]
    public int CarState { get; set; }

    [JsonPropertyName("nTEState")]
    public int TEState { get; set; }

    [JsonPropertyName("nAlarmState")]
    public int AlarmState { get; set; }

    [JsonPropertyName("strOther")]
    public string Other { get; set; }
}
