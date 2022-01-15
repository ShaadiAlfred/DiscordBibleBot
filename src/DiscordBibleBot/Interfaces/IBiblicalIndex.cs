namespace Interfaces;

public interface IBiblicalIndex<T>
{
    public T Book { get; set; }
    public int Chapter { get; set; }
    public int? Verse { get; set; }
    public int? VerseRange { get; set; }
}