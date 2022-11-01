# SinoTrack to Traccar data converter

## Description
CLI allows to dump data from [SinoTrack IOT platform](https://www.sinotrack.com) and insert it into your own [Traccar server](https://www.traccar.org)

## Usage
### Export
This command will login into SinoTract IOT platform and exports all replay records in specified period for device into JSON file.
File can be found in ``ReplayRecords`` folder with ``1234567890 2022-01-01 2023-01-01.json`` file name.

Example:
``` bash
export --server SinoTracking --login 1234567890 --password 1234567890 --device-id 1234567890 --start 2022-01-01T00:00:00Z --end 2023-01-01T00:00:00Z
```

### InsertPostgres
This command will read JSON file and insert exported replay records into Traccar PostgreSQL database directly.

Example:
``` bash
insert-postgres --file-name "1234567890 2022-01-01 2023-01-01.json" --host 127.0.0.1 --username 1234567890 --password 1234567890 --database traccar
```
