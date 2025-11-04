# Model Parser App

## Services

### LxfmlParser

Parses LXFML files and outputs data.

### ModelDynamoDbStorageService

Creates and Reads ModelInfo from dynamoDB.

Follows a `IModelStorageService` interface
The implementation `ModelDynamoDbStorageService` implementation also contains the `EnsureTableExists` functionality,
intended to be called by a (web) app when initializing the app, to ensure a table with the correct partition and sort
key exists.

When creating and injecting the service in the app, the `ModelDynamoDbStorageConfiguration` class allows the service
to be defined by the given sectionName, defined in `appSettings.json` and `appSettings.Development.json`.

```bash
services.AddOptions<ModelDynamoDbStorageConfiguration>()
.Bind(configuration.GetSection(ModelDynamoDbStorageConfiguration.SectionName));
```

# DynamoDb

The table is defined in `EnsureTableExists`.
It follows a simple table implementation, using the given model name as the partition key, and the total bricks counted
as a sort key.
This is not ideal, since the total brick count is not necessarily unique, but showcases how entries in DynamoDb can
have the same partition key, and therefore be sorted into the same partition when AWS scales the database.

This means that the (unique) primary key is the combination of partition key (name) + sort key (totalbricks).

Uploading a file with the same name, but a different brick count, will result in a new entry being made.

Uploading a file with the same name, and the same brick count, results in the entry being overwritten.

Unlike database options like SQL, DynamoDb can dynamically add new properties, as long as the required partition and
sort key is added.

## Considerations

I originally coupled the `ModelInfo` class more to `DynamoDB` by using its library to define properties and table name
on the class, but this worked against the table name service injection in the (web) app, and I felt this coupled
the type too closely to `DynamoDb`.

Sort key: The is not enough unique data in the file given to really justify a sort key, but I wanted to show off
the capability of DynamoDb. Using something like `version` instead of `totalBricks` would make more sense.

Generally, having a primary key composed of partition key (name) + sort key (version) allows versions of the same model
to be stored in the same partition (basically, when AWS allocates resources and servers to the data, the related data
with the same partition key (file name) stays together for easier and faster fetching, instead of queries having to
go through all entries to find them).
Alternatively, I could define an entry for each brick, and then they share the same partition key (file name).
I could also create a hash of the order of Brick uuids, and store the entries with that as the sort key - this would
essentially function as impromptu, simplified versioning.