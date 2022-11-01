namespace SinoTrackTraccarDataConverter.CLI.Models;

[Flags]
internal enum CarStateEnum
{
    Undefined = 0,
    Unknown1 = 1,
    Unknown2 = 2,
    Unknown4 = 4,
    CarShake = 8,
    Unknown16 = 16,
    DoorOpen = 32,
    Unknown64 = 64,
    EngineOn = 128,
    Unknown256 = 256,
    Refuel = 512,
    Heavy = 1024,
    Unknown = 2048,
    StartDefend = 4096,
    Uknown8192 = 8192,
    Uknown16384 = 16384,
    Unknown32768 = 32768,
    Unknown262144 = 262144,
    Unknown524288 = 524288,
    Unknown1048576 = 1048576,
    Unknown2097152 = 2097152,
    Unknown4194304 = 4194304,
    Unknown8388608 = 8388608
}
