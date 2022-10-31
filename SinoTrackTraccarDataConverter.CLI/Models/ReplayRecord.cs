using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SinoTrackTraccarDataConverter.CLI.Models;

internal class ReplayRecord
{
    [DisplayName("nID")]
    public int RecordId { get; set; }

    [DisplayName("strTEID")]
    public string TEID { get; set; }

    [JsonIgnore]
    [DisplayName("nTime")]
    public long UnixTimeSeconds { get; set; }

    public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(UnixTimeSeconds);

    [DisplayName("dbLon")]
    public decimal Longitude { get; set; }

    [DisplayName("dbLat")]
    public decimal Latitude { get; set; }

    [DisplayName("nDirection")]
    public int Direction { get; set; }

    [DisplayName("nSpeed")]
    public int Speed { get; set; }

    [DisplayName("nGSMSignal")]
    public int GSMSignal { get; set; }

    [DisplayName("nGPSSignal")]
    public int GPSSignal { get; set; }

    [DisplayName("nFuel")]
    public int Fuel { get; set; }

    [DisplayName("nMileage")]
    public int Mileage { get; set; }

    [DisplayName("nTemp")]
    public int Temp { get; set; }

    [DisplayName("nCarState")]
    public int CarState { get; set; }

    [DisplayName("nTEState")]
    public int TEState { get; set; }

    [DisplayName("nAlarmState")]
    public int AlarmState { get; set; }

    [DisplayName("strOther")]
    public string Other { get; set; }
}
