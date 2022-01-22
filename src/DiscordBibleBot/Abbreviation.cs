using System.Text.Json;

public class Abbreviation
{
    public string BookName { get; set; } = string.Empty;
    public List<string> Abbreviations { get; set; } = new List<string>();

    public static List<Abbreviation> GetAll()
    {
        var jsonFile = File.ReadAllText(
            Path.Combine(
                Environment.CurrentDirectory,
                "DiscordBibleBot.Data",
                "abbreviations.json"
            )
        );

        return JsonSerializer.Deserialize<List<Abbreviation>>(jsonFile) ??
            new List<Abbreviation>();
    }

    public override string ToString()
    {
        return Abbreviations.Count > 0 ?
            $"{BookName}: [{Abbreviations?.Aggregate((a, b) => a + ", " + b)}]" :
            $"{BookName}: []";
    }
}