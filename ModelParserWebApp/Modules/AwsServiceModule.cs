using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using ModelParserApp.Services;

namespace ModelParserWebApp.Modules;

public static class AwsServiceModule
{
    public static void AddAwsServices(this IServiceCollection services)
    {
        services.AddDefaultAWSOptions(new AWSOptions
        {
            // This is the name I've given to my personal account AWS configuration. Feel free to change.
            Profile = "personal",
            Region = RegionEndpoint.EUWest1
        });

        services.AddAWSService<IAmazonDynamoDB>();
    }

    public static void AddDynamoDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Binds the configuration settings from appsettings.json and appSettings.Development.json
        // Keep a singleton instance based on the table name (value - defined in appSettings) 
        var x = ModelDynamoDbStorageConfiguration.SectionName;
        var y = configuration.GetSection(x);
        Console.WriteLine("The sectionname: {0}",
            configuration.GetSection(ModelDynamoDbStorageConfiguration.SectionName).GetSection("ModelDynamoDbStorage"));

        services.AddOptions<ModelDynamoDbStorageConfiguration>()
            .Bind(configuration.GetSection(ModelDynamoDbStorageConfiguration.SectionName));


        services.AddSingleton<IModelStorageService, ModelDynamoDbStorageService>();
        // defined explicitly, mostly for when checking directly if its table exists, since that is not defined in the interface
        services.AddSingleton<ModelDynamoDbStorageService>();
    }
}