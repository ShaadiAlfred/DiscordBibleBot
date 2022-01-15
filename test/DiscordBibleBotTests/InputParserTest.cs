using System;
using Utils;
using Xunit;
using Xunit.Abstractions;

namespace DiscordBibleBotTests;

public class InputParserTest
{
    private readonly ITestOutputHelper output;

    public InputParserTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void InvalidBecauseDoesNotStartWithPrefix()
    {
        InputParser inputParser = new("hello world", "!b");

        Assert.Throws<ArgumentException>(() => inputParser.Parse());
    }

    [Fact]
    public void InvalidBecauseWrongPrefix()
    {
        InputParser inputParser = new("!d hello world", "!b");

        Assert.Throws<ArgumentException>(() => inputParser.Parse());
    }

    [Fact]
    public void ShouldPassStartsWithTheCorrectPrefix()
    {
        InputParser inputParser = new("!b John 1:2", "!b");
        var parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);


        inputParser = new("!b John 1:2-5", "!b");
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Equal(5, parsedBiblicalIndex.VerseRange);

        inputParser = new("!b 1 John 1:2", "!b");
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);

        inputParser = new("!b 1 John 1:2-4", "!b");
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Equal(4, parsedBiblicalIndex.VerseRange);
    }

    [Fact]
    public void ShouldNormalizeWhiteSpaces()
    {
        InputParser inputParser = new("!b   1John   1 : 2", "!b");
        var parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);


        inputParser = new("!b  1   John 1  : 2 - 5  ", "!b");
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Equal(5, parsedBiblicalIndex.VerseRange);
    }
}