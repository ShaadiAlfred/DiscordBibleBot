namespace Utils;

public class SuperscriptNumbers
{
    public static string Get(int num)
    {
        return new string[] {
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
        }[num];
    }
}