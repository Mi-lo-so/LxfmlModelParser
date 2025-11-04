namespace ModelParserApp.Models;

public class BoneInfo
{
    public string Id { get; set; } = "";

    /// <summary>
    ///     For example of type: "-0.0000003576279,0,-1,0,1,0,1,0,-0.0000003576279,4.4,0.02119982,-11.2"
    /// </summary>
    public string Transformation { get; set; } = "";
}