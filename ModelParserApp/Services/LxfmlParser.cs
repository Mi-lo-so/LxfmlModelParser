using System.Xml.Linq;
using ModelParserApp.Models;

namespace ModelParserApp.Services;

public static class LxfmlParser
{
    /// <summary>
    ///     Parses a stream and returns "Brick" elements with data
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static List<BrickInfo> ParseToBricks(Stream stream)
    {
        var doc = XDocument.Load(stream);
        var brickElements = doc.Descendants("Brick");

        return brickElements
            .Select(ParseBrick)
            .ToList();
    }

    private static BrickInfo ParseBrick(XElement brick)
    {
        return new BrickInfo
        {
            Id = (string?)brick.Attribute("uuid") ?? "",
            DesignId = (string?)brick.Attribute("designID") ?? "",
            Parts = brick.Elements("Part")
                .Select(ParsePart)
                .ToList()
        };
    }

    private static PartInfo ParsePart(XElement part)
    {
        var materialsAttr = (string?)part.Attribute("materials") ?? "";
        // Since other examples online show that multiple materials are possible, let's handle them
        var materials = materialsAttr
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(ParseMaterial)
            .ToList();

        var bones = part.Elements("Bone")
            .Select(ParseBone)
            .ToList();

        return new PartInfo
        {
            Id = (string?)part.Attribute("uuid") ?? "",
            DesignId = (string?)part.Attribute("designID") ?? "",
            Materials = materials,
            Bones = bones
        };
    }

    private static MaterialInfo ParseMaterial(string material)
    {
        var parts = material.Split(':');
        return new MaterialInfo
        {
            Id = parts.ElementAtOrDefault(0) ?? "",
            Layer = parts.ElementAtOrDefault(1) ?? ""
        };
    }

    private static BoneInfo ParseBone(XElement bone)
    {
        return new BoneInfo
        {
            Id = (string?)bone.Attribute("uuid") ?? "",
            Transformation = (string?)bone.Attribute("transformation") ?? ""
        };
    }
}