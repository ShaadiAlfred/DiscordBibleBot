using Utils;
using Xunit;

namespace DiscordBibleBotTests;

public class BiblicalIndexTest
{
    [Fact]
    public void ToStringReturnsCorrectFormat()
    {
        BiblicalIndex biblicalIndex = new BiblicalIndex("1 John", 1, 2);
        Assert.Equal("1 John 1:2", biblicalIndex.ToString());

        biblicalIndex = new BiblicalIndex("Mark", 4, 5);
        Assert.Equal("Mark 4:5", biblicalIndex.ToString());

        biblicalIndex = new BiblicalIndex("1 Samuel", 4, 5, 8);
        Assert.Equal("1 Samuel 4:5-8", biblicalIndex.ToString());
    }

}