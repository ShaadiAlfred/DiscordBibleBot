using System.Text.RegularExpressions;
namespace Utils;

public class BiblicalIndex
{
    public string Book { get; set; }
    public int Chapter { get; set; }
    public int Verse { get; set; }
    public int? VerseRange { get; set; }

    public BiblicalIndex(string book, int chapter, int verse, int? verseRange = null)
    {
        this.Book = book;
        this.Chapter = chapter;
        this.Verse = verse;
        this.VerseRange = verseRange;

        this.Normalize();
    }

    private void Normalize()
    {
        if (char.IsDigit(Book[0]))
        {
            if (!char.IsWhiteSpace(Book[1]))
            {
                this.Book = Book[0] + " " + Book.Substring(1);
                return;
            }

            this.Book = Regex.Replace(this.Book, @"\s\s+", " ");
        }
    }

    public override string ToString()
    {
        if (VerseRange is not null)
        {
            return $"{Book} {Chapter}:{Verse}-{VerseRange}";
        }

        return $"{Book} {Chapter}:{Verse}";
    }
}