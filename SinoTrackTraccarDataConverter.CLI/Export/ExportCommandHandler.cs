using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Text.Json;
using SinoTrackTraccarDataConverter.CLI.Models;
using SinoTrackTraccarDataConverter.CLI.ResponseModels;

namespace SinoTrackTraccarDataConverter.CLI.Export;

internal class ExportCommandHandler : ICommandHandler
{
    private readonly HttpClient _httpClient;

    public string Server { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string DeviceId { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }

    public ExportCommandHandler()
    {
        var httpClientHandler = new HttpClientHandler
        {
            AllowAutoRedirect = true,
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };

        _httpClient = new HttpClient(httpClientHandler);
    }

    public int Invoke(InvocationContext context)
    {
        return InvokeAsync(context).GetAwaiter().GetResult();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var cancellationToken = context.GetCancellationToken();
        var url = Constants.ServerNameToUrl[Server];

        context.Console.WriteLine("Logging in");
        var login = await LoginAsync(url, cancellationToken);
        if (login.Records.SelectMany(_ => _).Contains("-1"))
        {
            context.Console.Error.WriteLine("Login failed");
            return -1;
        }
        context.Console.WriteLine("Login success");

        var replays = new List<ReplayResponseModel>();
        var page = 0;
        var totalPages = 0;
        do
        {
            page++;

            if (totalPages == 0) context.Console.WriteLine($"Downloading {page} replay records page");
            else context.Console.WriteLine($"Downloading {page}/{totalPages} replay records page");

            var replay = await GetReplayAsync(url, page, cancellationToken);
            replays.Add(replay);

            totalPages = replay.TotalPages;
        } while (page != totalPages);
        context.Console.WriteLine("Replay records downloaded");

        context.Console.WriteLine("Mapping replay records");
        var replayRecords = MapToReplayRecords(replays);
        context.Console.WriteLine("Replay records mapped");

        var fileName = await SaveRecordsToFileAsync(replayRecords, cancellationToken);
        context.Console.WriteLine($"Replay records saved in {fileName} file");

        return 0;
    }

    private async Task<LoginResponseModel> LoginAsync(string url, CancellationToken cancellationToken = default)
    {
        var loginFormContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Cmd", "Proc_GetLoginType"),
            new KeyValuePair<string, string>("Data", $"N'{Login}',N'{Password}'"),
            new KeyValuePair<string, string>("Field", string.Empty),
            new KeyValuePair<string, string>("Server", string.Empty)
        });
        var loginRequestResult = await _httpClient.PostAsync(url, loginFormContent, cancellationToken);
        var loginStream = await loginRequestResult.Content.ReadAsStreamAsync(cancellationToken);
        var login = await JsonSerializer.DeserializeAsync<LoginResponseModel>(loginStream, cancellationToken: cancellationToken);

        return login;
    }

    private async Task<ReplayResponseModel> GetReplayAsync(string url, int page, CancellationToken cancellationToken = default)
    {
        var replayFormContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Cmd", "Proc_GetTrack"),
            new KeyValuePair<string, string>("Data", $"N'{DeviceId}',N'{Start.ToUnixTimeSeconds()}',N'{End.ToUnixTimeSeconds()}',N'100000'"),
            new KeyValuePair<string, string>("Field", string.Empty),
            new KeyValuePair<string, string>("Server", string.Empty),
            new KeyValuePair<string, string>("PageSize", "200"),
            new KeyValuePair<string, string>("AbsolutePage", $"{page}")
        });
        var replayRequestResult = await _httpClient.PostAsync(url, replayFormContent, cancellationToken);
        var replayStream = await replayRequestResult.Content.ReadAsStreamAsync(cancellationToken);
        var replay = await JsonSerializer.DeserializeAsync<ReplayResponseModel>(replayStream, cancellationToken: cancellationToken);

        return replay;
    }

    private async Task<string> SaveRecordsToFileAsync(ReplayRecord[] replayRecords, CancellationToken cancellationToken = default)
    {
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "ReplayRecords");
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        const string fileDateFormat = "yyyy-MM-dd";
        var fileName = $"{DeviceId} {Start.ToString(fileDateFormat)} {End.ToString(fileDateFormat)}.json";
        var filePath = Path.Combine(directoryPath, fileName);

        var fileStream = File.OpenWrite(filePath);
        await JsonSerializer.SerializeAsync(fileStream, replayRecords, cancellationToken: cancellationToken);

        return fileName;
    }

    private static ReplayRecord[] MapToReplayRecords(IEnumerable<ReplayResponseModel> replays)
    {
        var recordsFields = replays.First().RecordFields;
        var fieldIndexToProperty = new Dictionary<int, PropertyInfo>();
        for (var fieldIndex = 0; fieldIndex < recordsFields.Length; fieldIndex++)
        {
            var fieldName = recordsFields[fieldIndex];
            var property = typeof(ReplayRecord).GetProperties().Single(_ => _.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName == fieldName);

            fieldIndexToProperty.Add(fieldIndex, property);
        }

        var replayRecords = replays
            .SelectMany(_ => _.Records)
            .Select(_ => MapToReplayRecord(_, fieldIndexToProperty))
            .ToArray();

        return replayRecords;
    }

    private static ReplayRecord MapToReplayRecord(string[] rawRecord, Dictionary<int, PropertyInfo> fieldIndexToProperty)
    {
        var replayRecord = new ReplayRecord();
        for (var i = 0; i < rawRecord.Length; i++)
        {
            var rawRecordValue = rawRecord[i];
            var recordType = fieldIndexToProperty[i].PropertyType;
            var recordValue = Convert.ChangeType(rawRecordValue, recordType);
            fieldIndexToProperty[i].SetValue(replayRecord, recordValue);
        }

        return replayRecord;
    }
}
