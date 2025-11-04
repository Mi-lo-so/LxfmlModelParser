namespace ModelParserApp.Models;

public class BrickInfo
{
    public string Id { get; set; } = "";
    public string DesignId { get; set; } = "";
    public List<PartInfo> Parts { get; set; } = new();
}