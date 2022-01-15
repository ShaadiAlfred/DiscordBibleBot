using System.Globalization;
namespace Utils;

public class SuperscriptNumbers
{
    private readonly static string[] UNICODE_TABLE = new string[] {
        "\u2070",
        "\u00b9",
        "\u00b2",
        "\u00b3",
        "\u2074",
        "\u2075",
        "\u2076",
        "\u2077",
        "\u2078",
        "\u2079"
    };

    public static string ParseFrom(int num)
    {
        return num
            .ToString()
            .ToCharArray()
            .Select(x => UNICODE_TABLE[x - '0'])
            .Aggregate(string.Empty, (a, b) => a + b);
    }

}