# SinoTrack IOT platform to Traccar data converter / migration solution

## Description
CLI allows to export data from [SinoTrack IOT platform](https://www.sinotrack.com) to JSON file and insert it into your own [Traccar server](https://www.traccar.org) PostgreSQL database.

## Supported devices
CLI was developed with simple GPS trackers in mind (like ST-901 and ST-907) which use H02 protocol and doesn't provide much information to the server.
SinoTrack IOT platform supports many additional fields (like Fuel, Power, Ignition, etc), but I don't know how to properly map them.

## Before use
Please, backup your Traccar data before using this CLI.
I don't provide any warranty, support or coverage for your own actions using this tool.
Both SinoTrack IOT platform and Traccar could be updated with no backward compatibility in mind and corrupt the data - I can't predict this.

## Usage
### Export
This command will login into SinoTract IOT platform and dump all replay records in specified period for device into JSON file.
File can be found in ``ReplayRecords`` folder with ``1234567890 2022-01-01 2023-01-01.json`` file name.

CLI example:
``` pwsh
export --server SinoTracking --login USERNAME --password PASSWORD --device-id 1234567890 --start 2022-01-01T00:00:00Z --end 2023-01-01T00:00:00Z
```
Docker example:
``` pwsh
docker run -v C:\ReplayRecords:/app/ReplayRecords -it ghcr.io/illja96/sinotrack-traccar-data-converter:master export --server SinoTracking --login USERNAME --password PASSWORD --device-id 1234567890 --start 2022-01-01T00:00:00Z --end 2023-01-01T00:00:00Z
```

### Insert
There are no way to use [Traccar HTTP API](https://www.traccar.org/traccar-api/) because it doesn't contains any endpoints to insert position information.
Probably, there are possibility to emulate H02 protocol and inject data into H02 supported Traccar port directly.
I don't feel like this is are the best option, so current implementation will work only if you have access to Traccar database.
For now, only PostgreSQL database supported (because I use it, duh).

### InsertPostgres
This command will read JSON file with exported replay records and insert them into Traccar PostgreSQL database directly.

CLI example:
``` pwsh
insert-postgres --file-name "1234567890 2022-01-01 2023-01-01.json" --host 127.0.0.1 --username USERNAME --password PASSWORD --database traccar
```
Docker example:
``` pwsh
docker run -v C:\ReplayRecords:/app/ReplayRecords -it ghcr.io/illja96/sinotrack-traccar-data-converter:master insert-postgres --file-name "1234567890 2022-01-01 2023-01-01.json" --host 127.0.0.1 --username USERNAME --password PASSWORD --database traccar
```
