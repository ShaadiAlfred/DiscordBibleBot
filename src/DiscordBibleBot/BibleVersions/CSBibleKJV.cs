using System.Runtime.InteropServices;
using Interfaces;
using CSBible;
using System.Text;
using Utils;

namespace BibleVersions;

public class CSBibleKJV : IBibleVersion<CSBible.Book>
{
    private StringBuilder sb = new();

    public string GetPassage(IBiblicalIndex<CSBible.Book> iBiblicalIndex)
    {
        if (iBiblicalIndex.Verse is null)
        {
            AppendChapter(iBiblicalIndex);
        }
        else if (iBiblicalIndex.VerseRange is null)
        {
            AppendVerse(iBiblicalIndex);
        }
        else
        {
            AppendVerseRange(iBiblicalIndex);
        }

        sb.Append($"{Environment.NewLine}[{iBiblicalIndex}]");

        // Fix ~, and new line characters showing up when running on Linux
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            sb
                .Replace("\r\n", "\n")
                .Replace("\r", string.Empty)
                .Replace("~", string.Empty);
        }

        return sb.ToString();
    }

    private void AppendVerseRange(IBiblicalIndex<Book> iBiblicalIndex)
    {
        for (
            int verseNumber = iBiblicalIndex.Verse ?? default(int);
            verseNumber <= iBiblicalIndex.VerseRange;
            verseNumber++
        )
        {
            sb.Append(
                SuperscriptNumbers.ParseFrom(verseNumber)
            );

            sb.Append(
                Bible.GetVerse(
                    iBiblicalIndex.Book,
                    iBiblicalIndex.Chapter,
                    verseNumber
                )
            );

            sb.Append(' ');
        }
    }

    private void AppendVerse(IBiblicalIndex<Book> iBiblicalIndex)
    {
        sb.Append(
            CSBible.Bible.GetVerse(
                iBiblicalIndex.Book,
                iBiblicalIndex.Chapter,
                iBiblicalIndex.Verse ?? default(int)
            )
        );
    }

    private void AppendChapter(IBiblicalIndex<Book> iBiblicalIndex)
    {
        string[] chapter = CSBible.Bible.GetChapter(iBiblicalIndex.Book, iBiblicalIndex.Chapter, false);

        for (int verseNumber = 1; verseNumber < chapter.Length; verseNumber++)
        {
            sb.Append(SuperscriptNumbers.ParseFrom(verseNumber));
            sb.Append(chapter[verseNumber]);
            sb.Append(' ');
        }
    }
}