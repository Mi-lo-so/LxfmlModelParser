internal class Program
{
    private static void Main(string[] args)
    {
    }
}
/*
private static void Main(string[] args)
{
    Console.Write("Enter path to file: ");
    var path = Console.ReadLine();

    if (string.IsNullOrEmpty(path) || !File.Exists(path))
    {
        Console.WriteLine("LXFML File not found!");
        return;
    }

    var bricks = LxfmlParser.Parse(path);

    Console.WriteLine($"\nTotal Bricks: {bricks.Count}");
    var totalParts = bricks.Sum(b => b.Parts.Count);
    Console.WriteLine($"Total Parts: {totalParts}");

    var materialGroups = bricks
        .SelectMany(b => b.Parts)
        .GroupBy(p => p.Materials)
        .Select(g => new { Material = g.Key, Count = g.Count() });

    Console.WriteLine("\nMaterials Used:");
    foreach (var m in materialGroups)
        Console.WriteLine($"  - {m.Material}: {m.Count} parts");

    Console.WriteLine("\nDone!");
}
}*/