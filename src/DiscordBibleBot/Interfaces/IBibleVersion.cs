namespace Interfaces;

public interface IBibleVersion<TBook>
{
    public string GetPassage(IBiblicalIndex<TBook> iBiblicalIndex);
}