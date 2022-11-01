namespace SinoTrackTraccarDataConverter.CLI.Models;

/// <summary>
/// TODO: Find what this flag enum doing in SinoTrack IOT platform
/// This enum flags was scraped from compiled code on website
/// </summary>
[Flags]
internal enum Status2Enum
{
    Undefined = 0,
    RFIDSeal = 1,
    ButtonSeal = 2,
    Unknown4 = 4,
    Unknown8 = 8,
    TimingUpload = 16,
    Tear = 32,
    Seal = 64,
    Open = 128,
    RegCardRelieve = 256,
    RegCardSeal = 512,
    UnRegCardRelieve = 1024,
    UnRegCardSeal = 2048,
    BatteryLowVoltage = 4096,
    SMSRelieve = 8192,
    SMSSeal = 16384,
    RFIDRelieve = 32768,
    GoOut = 262144,
    EnterDoor = 524288,
    PlatformRelieve = 1048576,
    PlatformSeal = 2097152,
    NFCRelieve = 4194304,
    NFCSeal = 8388608
}
