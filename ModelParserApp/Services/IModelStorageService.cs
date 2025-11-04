using ModelParserApp.Models;

namespace ModelParserApp.Services;


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