using System.CommandLine;

namespace SinoTrackTraccarDataConverter.CLI.InsertPostgres;

internal class InsertPostgresCommand : Command
{
    internal Option<string> FileNameOption { get; set; }
    internal Option<string> HostOption { get; set; }
    internal Option<string> UsernameOption { get; set; }
    internal Option<string> PasswordOption { get; set; }
    internal Option<string> DatabaseOption { get; set; }

    public InsertPostgresCommand() : base("insert-postgres", "Insert exported data into Traccar Postgres database")
    {
        FileNameOption = new Option<string>("--file-name", "Filename with exported replay records")
        {
            IsRequired = true
        };
        AddOption(FileNameOption);

        HostOption = new Option<string>("--host", "PostgreSQL host name")
        {
            IsRequired = true
        };
        AddOption(HostOption);

        UsernameOption = new Option<string>("--username", "PostgreSQL username")
        {
            IsRequired = true
        };
        AddOption(UsernameOption);

        PasswordOption = new Option<string>("--password", "PostgreSQL password")
        {
            IsRequired = true
        };
        AddOption(PasswordOption);

        DatabaseOption = new Option<string>("--database", "PostgreSQL database")
        {
            IsRequired = true
        };
        AddOption(DatabaseOption);
    }
}
