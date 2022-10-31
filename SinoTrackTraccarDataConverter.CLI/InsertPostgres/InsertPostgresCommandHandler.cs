using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Text.Json;
using Npgsql;
using SinoTrackTraccarDataConverter.CLI.Models;

namespace SinoTrackTraccarDataConverter.CLI.InsertPostgres;

internal class InsertPostgresCommandHandler : ICommandHandler
{
    public string FileName { get; set; }
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }

    public int Invoke(InvocationContext context)
    {
        return InvokeAsync(context).GetAwaiter().GetResult();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var cancellationToken = context.GetCancellationToken();

        context.Console.WriteLine($"Opening {FileName} file");
        Stream fileStream;
        try
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.ReplayRecordsFolderName, FileName);
            fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        }
        catch (FileNotFoundException)
        {
            context.Console.Error.WriteLine("File not found");
            return -1;
        }
        context.Console.WriteLine("File opened");

        context.Console.WriteLine("Reading file");
        var replayRecords = await JsonSerializer.DeserializeAsync<ReplayRecord[]>(fileStream, cancellationToken: cancellationToken);
        var deviceId = replayRecords.First().TEID;
        context.Console.WriteLine($"{replayRecords.Length} replay records found");

        context.Console.WriteLine("Connecting to database");
        var connectionString = $"Host={Host};Username={Username};Password={Password};Database={Database}";
        await using var postgresConnection = new NpgsqlConnection(connectionString);
        await postgresConnection.OpenAsync(cancellationToken);

        context.Console.WriteLine("Selecting id of device");
        var deviceIdQuery = $"SELECT id FROM public.tc_devices WHERE uniqueid = {deviceId}";
        await using var deviceIdCommand = new NpgsqlCommand(deviceIdQuery, postgresConnection);
        var deviceDatabaseId = await deviceIdCommand.ExecuteScalarAsync(cancellationToken);
        context.Console.WriteLine("Found id of device");

        return 0;
    }
}
