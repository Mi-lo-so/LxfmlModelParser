using System.Text;
using ModelParserApp.Services;

namespace ModelParserUnitTests;

public class LxfmlParserTests
{
    [SetUp]
    public void Setup()
    {
    }


// TODO can we mock the behaviour of dynamoDB?

// TODO parses expected number of totalBricks and totalParts, and materials.

    [Test]
    public void Parse_ShouldReturnCorrectBrickInfo()
    {
        // Arrange: a small sample LXFML snippet
        var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <LXFML versionMajor=""9"" versionMinor=""0"" versionPatch=""0"">
                <Brick uuid='brick1' designID='123'>
                    <Part uuid='part1' designID='456' materials='107:0,14:1'>
                        <Bone uuid='bone1' transformation='-0.0000003576279,0,-1,0,1,0,1,0,-0.0000003576279,-3.6,0.3200001,3.6' />
                    </Part>
                </Brick>
            </LXFML>";
        using var mockedStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        // Act
        var bricks = LxfmlParser.ParseToBricks(mockedStream);

        // Assert
        Assert.That(bricks, Has.Count.EqualTo(1));

        var brick = bricks[0];
        Assert.That(brick.Id, Is.EqualTo("brick1"));
        Assert.That(brick.DesignId, Is.EqualTo("123"));
        Assert.That(brick.Parts, Has.Count.EqualTo(1));

        var part = brick.Parts[0];
        Assert.That(part.Id, Is.EqualTo("part1"));
        Assert.That(part.DesignId, Is.EqualTo("456"));

        // 2 materials were identified
        Assert.That(part.Materials, Has.Count.EqualTo(2));
        Assert.That(part.Materials[0].Id, Is.EqualTo("107"));
        Assert.That(part.Materials[0].Layer, Is.EqualTo("0"));
        Assert.That(part.Materials[1].Id, Is.EqualTo("14"));
        Assert.That(part.Materials[1].Layer, Is.EqualTo("1"));

        Assert.That(part.Bones, Has.Count.EqualTo(1));
        Assert.That(part.Bones[0].Id, Is.EqualTo("bone1"));
    }

    [Test]
    public void Parse_EmptyFile_ReturnsEmptyList()
    {
        using var mockedStream = new MemoryStream(Encoding.UTF8.GetBytes("<LXFML></LXFML>"));
        var result = LxfmlParser.ParseToBricks(mockedStream);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Parse_MissingAttributes_DoesNotThrow()
    {
        var xml = @"
            <LXFML>
                <Brick>
                    <Part />
                </Brick>
            </LXFML>";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        Assert.DoesNotThrow(() => LxfmlParser.ParseToBricks(stream));
    }
}