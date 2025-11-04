using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using ModelParserApp.Models;

namespace ModelParserApp.Services;

/// <summary>
///     Configuration class to be used when injecting service.
///     Defines the name of the table, and is used for fetching the correct service based on table name.
/// </summary>
public record ModelDynamoDbStorageConfiguration
{
    public const string SectionName = "ModelDynamoDbStorage"; // required for IOptions when injecting service in web api
    public required string TableName { get; init; }
}

/// <summary>
///     Interface, so the storage service can be reused for e.g. S3.
///     Makes testing easier, and injecting different services with the same functionality.
/// </summary>
public interface IModelStorageService
{
    Task CreateOrReplaceModel(ModelInfo data, bool overwrite = true);
    Task<List<ModelInfo>?> GetByName(string modelName);
    Task<List<ModelInfo>?> GetByNameAndBrickCount(string modelName, int brickCount);
    Task<List<ModelInfo>?> GetAll();
}

public class ModelDynamoDbStorageService(
    IOptions<ModelDynamoDbStorageConfiguration> configuration,
    IAmazonDynamoDB client)
    : IModelStorageService
{
    private readonly DynamoDBContext _context = new(client);

    public async Task CreateOrReplaceModel(ModelInfo data, bool overwrite = true)
    {
        Console.WriteLine("Name: {0}", data.Name);
        var fetch = await GetByNameAndBrickCount(data.Name, data.TotalBricks);

        if (fetch?.Count > 0 && !overwrite)
            throw new ArgumentException($"Model {data.Name} already exists, and overwrite is set to false.");

        var operationConfig = new SaveConfig { OverrideTableName = configuration.Value.TableName };
        await _context.SaveAsync(data, operationConfig);
    }

    public async Task<List<ModelInfo>?> GetAll()
    {
        var operationConfig = new ScanConfig { OverrideTableName = configuration.Value.TableName };
        var scan = _context.ScanAsync<ModelInfo>(new List<ScanCondition>(), operationConfig);
        var results = await scan.GetRemainingAsync();

        Console.WriteLine($"Found {results.Count} total items");
        return results;
    }

    public async Task<List<ModelInfo>?> GetByName(string modelName)
    {
        var queryConfig = new QueryConfig
        {
            OverrideTableName = configuration.Value.TableName
        };

        var search = _context.QueryAsync<ModelInfo>(
            modelName, // only search by the partition key
            queryConfig
        );

        var results = await search.GetRemainingAsync();
        Console.WriteLine("Found {0} items: {1} for modelName {2}", results.Count, results, modelName);
        return results;
    }

    public async Task<List<ModelInfo>?> GetByNameAndBrickCount(string modelName, int brickCount)
    {
        var operationConfig = new QueryConfig
        {
            OverrideTableName = configuration.Value.TableName,
            QueryFilter = [new ScanCondition("TotalBricks", ScanOperator.Equal, brickCount)]
        };
        var res = await _context
            .QueryAsync<ModelInfo>(modelName, operationConfig)
            .GetRemainingAsync();
        Console.WriteLine($"Found {res.Count} item(s) for '{modelName}' with {brickCount} bricks.");
        return res;
    }

    public async Task EnsureTableExists()
    {
        var tableName = configuration.Value.TableName;

        var tables = await client.ListTablesAsync();
        if (!tables.TableNames.Contains(tableName))
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                // Define attributes 
                // DynamoDB is generally open to have additional attributes added dynamically,
                // So these are only defined so we can specify that they're the partition and sort key.
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    // Partition key - by using name, duplicates will be in the same server partition.
                    new("Name", ScalarAttributeType.S),
                    // sort key - just to show that it's possible, since there are no good sortkey options 
                    // something like 'version' would be more practical.
                    new("TotalBricks", ScalarAttributeType.N)
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new("Name", KeyType.HASH), // Partition key
                    new("TotalBricks", KeyType.RANGE) // Sort key 
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
            };

            await client.CreateTableAsync(request);

            await WaitForTableToExist(tableName);
        }
    }

    private async Task WaitForTableToExist(string tableName)
    {
        string status;
        do
        {
            await Task.Delay(2000);
            var response = await client.DescribeTableAsync(tableName);
            status = response.Table.TableStatus;
        } while (status != TableStatus.ACTIVE);
    }
}