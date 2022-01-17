using System;
using BibleVersions;
using Utils;
using Xunit;

namespace DiscordBibleBotTests;

public class CSBibleKJVTest
{
    private CSBibleKJV cSBibleKJV = new();

    [Fact]
    public void ShouldGetOneVerse()
    {
        string mark1_3 = $"The voice of one crying in the wilderness, Prepare ye the way of the Lord, make his paths straight.{Environment.NewLine}[Mark 1:3]";

        Assert.Equal(mark1_3, cSBibleKJV.GetPassage(new BiblicalIndex("Mark", 1, 3)));
    }

    [Fact]
    public void ShouldGetAPassage()
    {
        string mark1_3_5 = $"³The voice of one crying in the wilderness, Prepare ye the way of the Lord, make his paths straight. ⁴John did baptize in the wilderness, and preach the baptism of repentance for the remission of sins. ⁵And there went out unto him all the land of Judaea, and they of Jerusalem, and were all baptized of him in the river of Jordan, confessing their sins.{Environment.NewLine}[Mark 1:3-5]";

        Assert.Equal(mark1_3_5, cSBibleKJV.GetPassage(new BiblicalIndex("Mark", 1, 3, 5)));
    }

    [Fact]
    public void ShouldGetWholeChapter()
    {
        string psalm_1 = $"¹Blessed is the man that walketh not in the counsel of the ungodly, nor standeth in the way of sinners, nor sitteth in the seat of the scornful. ²But his delight is in the law of the LORD; and in his law doth he meditate day and night. ³And he shall be like a tree planted by the rivers of water, that bringeth forth his fruit in his season; his leaf also shall not wither; and whatsoever he doeth shall prosper. ⁴The ungodly are not so: but are like the chaff which the wind driveth away. ⁵Therefore the ungodly shall not stand in the judgment, nor sinners in the congregation of the righteous. ⁶For the LORD knoweth the way of the righteous: but the way of the ungodly shall perish.{Environment.NewLine}[Psalms 1]";

        Assert.Equal(psalm_1, cSBibleKJV.GetPassage(new BiblicalIndex("Psalms", 1)));
    }
}