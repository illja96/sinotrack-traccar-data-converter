namespace SinoTrackTraccarDataConverter.CLI.Models;

/// <summary>
/// TODO: Find what this flag enum doing in SinoTrack IOT platform
/// This enum flags was scraped from compiled code on website
/// </summary>
[Flags]
internal enum Status1Enum
{
    Undefined = 0,
    GPSShortCircuit = 1,
    GPSOpenCircuit = 2,
    GPSFault = 4,
    FadezoneMakeup2 = 8,
    BatteryFault = 16,
    NetworkRoam = 32,
    FadezoneMakeup = 64,
    InvalidPosition = 128,
    CameraFault = 256,
    TTSFault = 512,
    LCDFault = 1024,
    PowerSaving = 2048,
    WifiLocation = 4096,
    LBS = 8192,
    Uknown16384 = 16384,
    Unknown32768 = 32768,
    Unknown262144 = 262144,
    Unknown524288 = 524288,
    Unknown1048576 = 1048576,
    Unknown2097152 = 2097152,
    Unknown4194304 = 4194304,
    Unknown8388608 = 8388608,
    MultLBS = 268435456,
    BatteryPower = 536870912,
    Shutdown = 1073741824,
    // Dormancy = 2147483648
}
