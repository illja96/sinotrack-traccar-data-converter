namespace SinoTrackTraccarDataConverter.CLI;

internal static class Constants
{
    internal static Dictionary<string, string> ServerNameToUrl = new()
    {
        { "SinoTrack", "https://www.sinotrack.com/APP/AppJson.asp" },
        { "VIP.SinoTrack", "https://242.sinotrack.com/APP/AppJson.asp" },
        { "SinoTracker", "https://217.sinotrack.com/APP/AppJson.asp" },
        { "SinoTracking", "https://101.sinotrack.com/APP/AppJson.asp" },
        { "SinoTrackPro", "https://245.sinotrack.com/APP/AppJson.asp" },
    };

    internal static string ReplayRecordsFolderName = "ReplayRecords";
}
