using ModelParserApp.Services;

namespace ModelParserApp.Modules;

/// <summary>
///     Any checks that needs to be run when starting the web app.
/// </summary>
public static class DynamoDbExtensionModule
{
    public static void InitializeDynamoDb(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dynamoServices = scope.ServiceProvider.GetRequiredService<ModelDynamoDbStorageService>();
            dynamoServices.EnsureTableExists().Wait();
        }
    }
}