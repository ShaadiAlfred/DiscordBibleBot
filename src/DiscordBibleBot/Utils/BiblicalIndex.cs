using System.Text;
using System.Text.RegularExpressions;
using CSBibleBook = CSBible.Book;

namespace Utils;

public class BiblicalIndex
{
    public string Book { get; set; }
    public CSBibleBook CSBibleBookName { get; set; }
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
        this.SetCSBibleBook();
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

        this.Book = string.Join(' ',
            this.Book
            .ToLower()
            .Split(' ')
            .Select(str =>
            {
                StringBuilder sb = new(str);
                sb[0] = char.ToUpper(sb[0]);
                return sb.ToString();
            })
        );
    }

    public void SetCSBibleBook()
    {
        string csbibleBookName;

        char firstCharacter = this.Book[0];
        if (char.IsDigit(firstCharacter))
        {
            switch (firstCharacter)
            {
                case '1':
                    csbibleBookName = "First_" + this.Book.Substring(2);
                    break;

                case '2':
                    csbibleBookName = "Second_" + this.Book.Substring(2);
                    break;

                case '3':
                    csbibleBookName = "Third_" + this.Book.Substring(2);
                    break;

                default:
                    throw new ArgumentException("Can't find this book");
            }
        }
        else
        {
            if (this.Book == "Song Of Solomon" || this.Book == "Song Of Songs")
            {
                csbibleBookName = "SongofSolomon";
            }
            else
            {
                csbibleBookName = this.Book;
            }
        }


        try
        {
            this.CSBibleBookName = (CSBibleBook)Enum.Parse(typeof(CSBibleBook), csbibleBookName);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException($"Can't find the book of '{Book}'");
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