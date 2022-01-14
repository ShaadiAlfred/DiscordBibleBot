namespace Utils;

public class BiblicalIndex
{
    public string Book { get; set; }
    public int Chapter { get; set; }
    public int Verse { get; set; }
    public int? VerseRange { get; set; }

    public BiblicalIndex(string book, int chapter, int verse, int? verseRange)
    {
        this.Book = book;
        this.Chapter = chapter;
        this.Verse = verse;
        this.VerseRange = verseRange;
    }
}