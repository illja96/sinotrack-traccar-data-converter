using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SinoTrackTraccarDataConverter.CLI.Export;

var root = new RootCommand("SinoTrack to Traccar data converter");
root.Add(new ExportCommand());

var parser = new CommandLineBuilder(root)
    .UseHost(_ => new HostBuilder(), (builder) => builder
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHttpClient();
        })
        .UseCommandHandler<ExportCommand, ExportCommandHandler>())
    .UseDefaults()
    .Build();

await parser.InvokeAsync(args);