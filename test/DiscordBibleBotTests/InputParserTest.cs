using System;
using System.Collections.Generic;
using Utils;
using Xunit;
using Xunit.Abstractions;

namespace DiscordBibleBotTests;

public class InputParserTest
{
    private readonly ITestOutputHelper output;
    private readonly List<Abbreviation> abbreviations = Abbreviation.GetAll();

    public InputParserTest(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void InvalidBecauseDoesNotStartWithPrefix()
    {
        InputParser inputParser = new("hello world", "!b", abbreviations);

        Assert.Throws<ArgumentException>(() => inputParser.Parse());
    }

    [Fact]
    public void InvalidBecauseWrongPrefix()
    {
        InputParser inputParser = new("!d hello world", "!b", abbreviations);

        Assert.Throws<ArgumentException>(() => inputParser.Parse());
    }

    [Fact]
    public void ShouldPassStartsWithTheCorrectPrefix()
    {
        InputParser inputParser = new("!b John 1:2", "!b", abbreviations);
        var parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);


        inputParser = new("!b John 1:2-5", "!b", abbreviations);
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Equal(5, parsedBiblicalIndex.VerseRange);

        inputParser = new("!b 1 John 1:2", "!b", abbreviations);
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);

        inputParser = new("!b 1 John 1:2-4", "!b", abbreviations);
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Equal(4, parsedBiblicalIndex.VerseRange);

        inputParser = new("!b Ezekiel 15:2", "!b", abbreviations);
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("Ezekiel", parsedBiblicalIndex.BookTitle);
        Assert.Equal(15, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);

        inputParser = new("!b 1st cor 1:1-15", "!b", abbreviations);
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 Corinthians", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(1, parsedBiblicalIndex.Verse);
        Assert.Equal(15, parsedBiblicalIndex.VerseRange);
    }

    [Fact]
    public void ShouldNormalizeWhiteSpaces()
    {
        InputParser inputParser = new("!b   1John   1 : 2", "!b", abbreviations);
        var parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);


        inputParser = new("!b  1   John 1  : 2 - 5  ", "!b", abbreviations);
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Equal(5, parsedBiblicalIndex.VerseRange);
    }

    [Fact]
    public void ShouldNormalizeCase()
    {

        InputParser inputParser = new("!b 1 JOHN 1:2", "!b", abbreviations);
        var parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(2, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);

        inputParser = new("!b mark 4:5", "!b", abbreviations);
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("Mark", parsedBiblicalIndex.BookTitle);
        Assert.Equal(4, parsedBiblicalIndex.Chapter);
        Assert.Equal(5, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);

        inputParser = new("!b 1 sAmUeL 4:5-8", "!b", abbreviations);
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 Samuel", parsedBiblicalIndex.BookTitle);
        Assert.Equal(4, parsedBiblicalIndex.Chapter);
        Assert.Equal(5, parsedBiblicalIndex.Verse);
        Assert.Equal(8, parsedBiblicalIndex.VerseRange);
    }

    [Fact]
    public void ShouldParseAbbreviations()
    {

        InputParser inputParser = new("!b 1 jn 1:1", "!b", abbreviations);
        var parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("1 John", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(1, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);

        inputParser = new("!b mk 4:5", "!b", abbreviations);
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("Mark", parsedBiblicalIndex.BookTitle);
        Assert.Equal(4, parsedBiblicalIndex.Chapter);
        Assert.Equal(5, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);

        inputParser = new("!b song 1:1", "!b", abbreviations);
        parsedBiblicalIndex = inputParser.Parse();
        Assert.Equal("Song Of Songs", parsedBiblicalIndex.BookTitle);
        Assert.Equal(1, parsedBiblicalIndex.Chapter);
        Assert.Equal(1, parsedBiblicalIndex.Verse);
        Assert.Null(parsedBiblicalIndex.VerseRange);
    }

}