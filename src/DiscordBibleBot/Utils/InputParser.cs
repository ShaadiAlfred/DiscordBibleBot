using System.Text;
using System.Text.RegularExpressions;

namespace Utils;

public class InputParser
{
    private string input = String.Empty;
    private string prefix = String.Empty;
    private List<Abbreviation> abbreviations;

    public InputParser(string input, string prefix, List<Abbreviation> abbreviations)
    {
        this.input = input.Trim();
        this.prefix = prefix;
        this.abbreviations = abbreviations;
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
        const string regex =
            @"(?<book>(\d(st|nd|rd|th)*)*\s*[a-zA-Z]+)\s*(?<chapter>\d+)\s*(:\s*(?<verse>\d+))?\s*(-\s*(?<verseRange>\d+))?";

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

    private string Normalize(string str)
    {
        if (char.IsDigit(str[0]))
        {
            switch (str.Substring(0, 3).ToLower())
            {
                case "1st":
                case "2nd":
                case "3rd":
                case "4th":
                    if (!char.IsWhiteSpace(str[3]))
                    {
                        str = str.Substring(0, 3) + " " + str.Substring(3);
                    }
                    break;
                default:
                    if (!char.IsWhiteSpace(str[1]))
                    {

                        str = str.Substring(0, 1) + " " + str.Substring(1);
                    }
                    break;
            }
        }

        return string.Join(' ',
            Regex.Replace(str, @"\s\s+", " ")
            .ToLower()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(str =>
            {
                StringBuilder sb = new(str);
                sb[0] = char.ToUpper(sb[0]);
                return sb.ToString();
            })
        );
    }

    private string ParseAbbreviation(string abbreviation)
    {
        string bookName = Normalize(abbreviation);

        var match = abbreviations.Find(
            book => book.Abbreviations.Find(abb => abb.Equals(bookName.ToLower())) is not null
        );

        if (match is not null)
        {
            bookName = match.BookName;
        }

        return bookName;
    }

    private string ParseBookName(string bookName)
    {
        return ParseAbbreviation(bookName);
    }

    public BiblicalIndex Parse()
    {
        RemovePrefix();

        var splitArguments = SplitInput();

        int? verse = null;
        int? verseRange = null;

        int temp;

        if (int.TryParse(splitArguments["verse"], out temp)) verse = temp;
        if (int.TryParse(splitArguments["verseRange"], out temp)) verseRange = temp;

        return new BiblicalIndex(
            ParseBookName(splitArguments["book"]),
            int.Parse(splitArguments["chapter"]),
            verse,
            verseRange
        );
    }
}