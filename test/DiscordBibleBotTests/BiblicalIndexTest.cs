using System;
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

    [Fact]
    public void ShouldNormalizeCase()
    {
        BiblicalIndex biblicalIndex = new BiblicalIndex("1 JOHN", 1, 2);
        Assert.Equal("1 John 1:2", biblicalIndex.ToString());

        biblicalIndex = new BiblicalIndex("mark", 4, 5);
        Assert.Equal("Mark 4:5", biblicalIndex.ToString());

        biblicalIndex = new BiblicalIndex("1 sAmUeL", 4, 5, 8);
        Assert.Equal("1 Samuel 4:5-8", biblicalIndex.ToString());
    }

    [Fact]
    public void ShouldConvertToCSBibleBookEnum()
    {
        BiblicalIndex biblicalIndex = new BiblicalIndex("1 John", 1, 2);
        Assert.Equal(CSBible.Book.First_John, biblicalIndex.Book);

        biblicalIndex = new BiblicalIndex("2 John", 1, 1);
        Assert.Equal(CSBible.Book.Second_John, biblicalIndex.Book);

        biblicalIndex = new BiblicalIndex("3 John", 4, 5, 8);
        Assert.Equal(CSBible.Book.Third_John, biblicalIndex.Book);

        biblicalIndex = new BiblicalIndex("Song of Songs", 4, 5, 8);
        Assert.Equal(CSBible.Book.SongofSolomon, biblicalIndex.Book);

        biblicalIndex = new BiblicalIndex("Song of Solomon", 4, 5, 8);
        Assert.Equal(CSBible.Book.SongofSolomon, biblicalIndex.Book);

        biblicalIndex = new BiblicalIndex("Psalm", 1);
        Assert.Equal(CSBible.Book.Psalms, biblicalIndex.Book);
    }

    [Fact]
    public void ShouldFailBecauseBookDoesNotExist()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            BiblicalIndex biblicalIndex = new BiblicalIndex("Enoch", 1, 1);
        });
    }
}