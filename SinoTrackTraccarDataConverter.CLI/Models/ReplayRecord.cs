﻿using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SinoTrackTraccarDataConverter.CLI.Models;

internal class ReplayRecord
{
    /// <summary>
    /// SinoTrack record ID
    /// </summary>
    [DisplayName("nID")]
    public int RecordId { get; set; }

    /// <summary>
    /// Device ID
    /// </summary>
    [DisplayName("strTEID")]
    public string TEID { get; set; }

    /// <summary>
    /// Record unix timestamp
    /// </summary>
    [JsonIgnore]
    [DisplayName("nTime")]
    public long UnixTimeSeconds { get; set; }

    /// <summary>
    /// Record datetime
    /// </summary>
    public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(UnixTimeSeconds);

    /// <summary>
    /// Longitude
    /// Range: -180 - 180
    /// </summary>
    [DisplayName("dbLon")]
    public decimal Longitude { get; set; }

    /// <summary>
    /// Latitude
    /// Range: -90 - 90
    /// </summary>
    [DisplayName("dbLat")]
    public decimal Latitude { get; set; }

    /// <summary>
    /// Direction (compass degrees)
    /// Range: 0 - 360
    /// </summary>
    [DisplayName("nDirection")]
    public int Direction { get; set; }

    /// <summary>
    /// Speed (km/h)
    /// Range: 0 - infinity
    /// </summary>
    [DisplayName("nSpeed")]
    public int Speed { get; set; }

    /// <summary>
    /// TODO: Unknown
    /// </summary>
    [DisplayName("nGSMSignal")]
    public int GSMSignal { get; set; }

    /// <summary>
    /// TODO: Unknown
    /// </summary>
    [DisplayName("nGPSSignal")]
    public int GPSSignal { get; set; }

    /// <summary>
    /// TODO: Unknown
    /// </summary>
    [DisplayName("nFuel")]
    public int Fuel { get; set; }

    /// <summary>
    /// Mileage (meters)
    /// Range: 0 - infinity
    /// </summary>
    [DisplayName("nMileage")]
    public int Mileage { get; set; }

    /// <summary>
    /// TODO: Unknown
    /// </summary>
    [DisplayName("nTemp")]
    public int Temp { get; set; }

    /// <summary>
    /// TODO: Unknown
    /// </summary>
    [DisplayName("nCarState")]
    public int CarState { get; set; }

    /// <summary>
    /// TODO: Unknown
    /// </summary>
    [DisplayName("nTEState")]
    public int TEState { get; set; }

    /// <summary>
    /// TODO: Unknown
    /// </summary>
    [DisplayName("nAlarmState")]
    public int AlarmState { get; set; }

    /// <summary>
    /// TODO: Unknown
    /// </summary>
    [DisplayName("strOther")]
    public string Other { get; set; }
}
