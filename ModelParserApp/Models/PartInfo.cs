namespace ModelParserApp.Models;

public class PartInfo
{
    public string Id { get; set; } = "";
    public string DesignId { get; set; } = "";
    public List<MaterialInfo> Materials { get; set; } = new();
    public List<BoneInfo> Bones { get; set; } = new();
}