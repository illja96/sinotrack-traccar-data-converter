using System.CommandLine;

namespace SinoTrackTraccarDataConverter.CLI.Export;

internal class ExportCommand : Command
{
    internal readonly Option<string> ServerOption;
    internal readonly Option<string> LoginOption;
    internal readonly Option<string> PasswordOption;
    internal readonly Option<string> DeviceIdOption;
    internal readonly Option<DateTimeOffset> StartOption;
    internal readonly Option<DateTimeOffset> EndOption;

    public ExportCommand() : base("export", "Export data from SinoTrack IOT platform")
    {
        ServerOption = new Option<string>(new[] { "--server" }, "Server")
        {
            IsRequired = true
        };
        ServerOption.AddValidator(_ =>
        {
            var value = _.GetValueOrDefault() as string;
            if (!Constants.ServerNameToUrl.ContainsKey(value)) _.ErrorMessage = $"Invalid server. Allowed values: {string.Join(',', Constants.ServerNameToUrl.Keys)}";
        });
        Add(ServerOption);

        LoginOption = new Option<string>(new[] { "--login" }, "Login")
        {
            IsRequired = true
        };
        Add(LoginOption);

        PasswordOption = new Option<string>(new[] { "--password" }, "Password")
        {
            IsRequired = true
        };
        Add(PasswordOption);

        DeviceIdOption = new Option<string>(new[] { "--device-id" }, "Device id")
        {
            IsRequired = true
        };
        Add(DeviceIdOption);

        StartOption = new Option<DateTimeOffset>(new[] { "--start" }, "Start of export period (ISO 8601)")
        {
            IsRequired = true
        };
        Add(StartOption);

        EndOption = new Option<DateTimeOffset>(new[] { "--end" }, "End of export period (ISO 8601)")
        {
            IsRequired = true
        };
        Add(EndOption);
    }
}
