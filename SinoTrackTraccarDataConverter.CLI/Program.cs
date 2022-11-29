using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Hosting;
using SinoTrackTraccarDataConverter.CLI.Export;
using SinoTrackTraccarDataConverter.CLI.InsertPostgres;

var root = new RootCommand("SinoTrack to Traccar data converter / migration solution")
{
    new ExportCommand(),
    new InsertPostgresCommand()
};

var parser = new CommandLineBuilder(root)
    .UseHost(_ => new HostBuilder(), (builder) => builder
        .UseCommandHandler<ExportCommand, ExportCommandHandler>()
        .UseCommandHandler<InsertPostgresCommand, InsertPostgresCommandHandler>())
    .UseDefaults()
    .Build();

await parser.InvokeAsync(args);