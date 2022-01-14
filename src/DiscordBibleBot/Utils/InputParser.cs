using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
namespace Utils;

public class InputParser
{
    private string input = String.Empty;
    private string prefix = String.Empty;

    public InputParser(string input, string prefix)
    {
        this.input = input.Trim();
        this.prefix = prefix;
    }

    public bool IsValid()
    {
        return input.StartsWith(prefix);
    }

    private void RemovePrefix()
    {
        if (!IsValid())
            throw new ArgumentException("Does not start with the correct prefix");

        this.input = input.Replace(prefix, String.Empty).Trim();
    }

    private Dictionary<string, string> SplitInput()
    {
        const string regex = @"(?<book>\d*\s*\w+)\s*(?<chapter>\d+)\s*:\s*(?<verse>\d+)\s*(-\s*(?<verseRange>\d+))?";

        Match match;

        if ((match = Regex.Match(input, regex)).Success)
        {
            return match.Groups.Cast<Group>().ToDictionary(g => g.Name, g => g.Value);
        }
        else
        {
            throw new ArgumentException("Does not match any regex");
        }
    }

    public BiblicalIndex Parse()
    {
        // Examples:
        // John 1:1
        // Mat 2:1-1
        // 1 John 1:1
        // 1 John 1:1-2
        RemovePrefix();

        var parsedArguments = SplitInput();

        int? verseRange = null;

        try
        {
            verseRange = int.Parse(parsedArguments["verseRange"]);
        }
        catch (Exception)
        { }

        return new BiblicalIndex(
            parsedArguments["book"],
            int.Parse(parsedArguments["chapter"]),
            int.Parse(parsedArguments["verse"]),
            verseRange
        );
    }
}