namespace ModelParserApp.Models;

// It's possible to use something like [DynamoDBTable("table name)], but it often works fine as a generic class as well. 
public class ModelInfo
{
    public string ModelId { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int TotalBricks { get; set; }
    public int TotalParts { get; set; }
    public List<MaterialInfo> Materials { get; set; } = new();
    public List<BrickInfo> Bricks { get; set; } = new();

    /// <summary>
    ///     Takes data and returns a ModelInfo type object from it.
    /// </summary>
    /// <param name="bricks">The bricks-data to store in a storage service.</param>
    /// <param name="name">Name to store model bricks under - works as the partition key</param>
    /// <param name="description">Optional description</param>
    /// <returns></returns>
    public static ModelInfo From(List<BrickInfo> bricks, string name, string? description)
    {
        return new ModelInfo
        {
            Name = name,
            Description = description,
            Bricks = bricks,
            TotalBricks = bricks.Count,
            TotalParts = bricks.Sum(b => b.Parts.Count),
            Materials = bricks
                .SelectMany(b => b.Parts)
                .SelectMany(p => p.Materials)
                .Distinct()
                .ToList()
        };
    }
}