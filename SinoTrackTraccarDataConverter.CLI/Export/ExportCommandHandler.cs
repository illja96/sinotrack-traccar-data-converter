using SinoTrackTraccarDataConverter.CLI.Models;
using System.CommandLine.Invocation;
using System.Net;
using System.Text.Json;

namespace SinoTrackTraccarDataConverter.CLI.Export;

internal class ExportCommandHandler : ICommandHandler
{
    private readonly HttpClient _httpClient;

    public string Server { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public ulong DeviceId { get; set; }
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

        var loginFormContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Cmd", "Proc_GetLoginType"),
            new KeyValuePair<string, string>("Data", $"N'{Login}',N'{Password}'"),
            new KeyValuePair<string, string>("Field", string.Empty),
            new KeyValuePair<string, string>("Server", string.Empty)
        });
        var loginRequestResult = await _httpClient.PostAsync(url, loginFormContent, cancellationToken);

        var replayFormContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("Cmd", "Proc_GetTrack"),
            new KeyValuePair<string, string>("Data", $"N'{DeviceId}',N'{Start.ToUnixTimeSeconds()}',N'{End.ToUnixTimeSeconds()}',N'100000'"),
            new KeyValuePair<string, string>("Field", string.Empty),
            new KeyValuePair<string, string>("Server", string.Empty),
            new KeyValuePair<string, string>("PageSize", "200"),
            new KeyValuePair<string, string>("AbsolutePage", "1")
        });
        var replayRequestResult = await _httpClient.PostAsync(url, replayFormContent, cancellationToken);
        var replayStream = await replayRequestResult.Content.ReadAsStreamAsync(cancellationToken);
        var replay = await JsonSerializer.DeserializeAsync<ReplayResponseModel>(replayStream, cancellationToken: cancellationToken);

        throw new NotImplementedException();
    }
}
