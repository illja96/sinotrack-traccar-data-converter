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
        var deviceIdQuery = $"SELECT id FROM public.tc_devices WHERE uniqueid = '{deviceId}'";
        await using var deviceIdCommand = new NpgsqlCommand(deviceIdQuery, postgresConnection);
        var rawDeviceDatabaseId = await deviceIdCommand.ExecuteScalarAsync(cancellationToken);
        if (rawDeviceDatabaseId == null)
        {
            context.Console.Error.WriteLine($"Device with {deviceId} as unique id doesn't exist in database");
            return -1;
        }

        var deviceDatabaseId = (int)rawDeviceDatabaseId;
        context.Console.WriteLine("Found id of device");

        context.Console.WriteLine("Inserting replay record into table");
        for (var i = 0; i < replayRecords.Length; i++)
        {
            var replayRecord = replayRecords[i];
            var positionQuery = CreateInsertPositionQuery(deviceDatabaseId, replayRecord);
            await using var positionCommand = new NpgsqlCommand(positionQuery, postgresConnection);
            await positionCommand.ExecuteNonQueryAsync(cancellationToken);
            context.Console.WriteLine($"{i}/{replayRecords.Length} replay record inserted");
        }
        context.Console.WriteLine("Replay record inserted");


        context.Console.WriteLine("Selecting last position record");
        var lastPositionQuery = $"SELECT id FROM tc_positions WHERE deviceid = '{deviceDatabaseId}' ORDER BY devicetime DESC LIMIT 1";
        await using var lastPositionCommand = new NpgsqlCommand(lastPositionQuery, postgresConnection);
        await using var lastPositionRecordReader = await lastPositionCommand.ExecuteReaderAsync(cancellationToken);
        await lastPositionRecordReader.ReadAsync(cancellationToken);
        var lastPositionRecordId = lastPositionRecordReader.GetInt32(0);
        var lastPositionRecordTime = replayRecords.Last().DateTime.ToString(Constants.TimestampWithoutTimeZoneFormat);
        await lastPositionRecordReader.CloseAsync();
        context.Console.WriteLine("Found last position record");

        context.Console.WriteLine("Updating device record");
        var deviceUpdateQuery = $"UPDATE tc_devices SET positionid='{lastPositionRecordId}', lastupdate='{lastPositionRecordTime}'";
        await using var deviceUpdateCommand = new NpgsqlCommand(deviceUpdateQuery, postgresConnection);
        await deviceUpdateCommand.ExecuteNonQueryAsync(cancellationToken);
        context.Console.WriteLine("Device record updated");

        return 0;
    }

    private static string CreateInsertPositionQuery(int deviceDatabaseId, ReplayRecord replayRecord)
    {
        var attributes = new
        {
            totalDistance = replayRecord.Mileage / 1000.0,
            migratedFrom = "SinoTrack IOT"
        };
        var attributesJson = JsonSerializer.Serialize(attributes);

        var timestampWithoutTimeZone = replayRecord.DateTime.ToString(Constants.TimestampWithoutTimeZoneFormat);
        var fieldValueDictionary = new Dictionary<string, string>()
        {
            { "protocol", "'h02'" },
            { "deviceid", $"'{deviceDatabaseId}'" },
            { "servertime", $"'{timestampWithoutTimeZone}'" },
            { "devicetime", $"'{timestampWithoutTimeZone}'" },
            { "fixtime", $"'{timestampWithoutTimeZone}'" },
            { "valid", "'true'" },
            { "latitude", $"'{replayRecord.Latitude}'" },
            { "longitude", $"'{replayRecord.Longitude}'" },
            { "altitude", "'0'" },
            { "speed", $"'{replayRecord.Speed}'" },
            { "course", $"'{replayRecord.Direction}'" },
            { "address", "null" },
            { "attributes", $"'{attributesJson}'" },
            { "accuracy", "'0'" },
            { "network", "null" },
        };

        var fields = string.Join(", ", fieldValueDictionary.Keys);
        var values = string.Join(", ", fieldValueDictionary.Values);

        var positionQuery = $"INSERT INTO tc_positions ({fields}) VALUES ({values})";
        return positionQuery;
    }
}
