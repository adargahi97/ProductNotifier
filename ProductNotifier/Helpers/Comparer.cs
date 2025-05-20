using ProductNotifier.Models;
namespace ProductNotifier.Helpers;

public class Comparer : IEqualityComparer<Record>
{
    public bool Equals(Record x, Record y)
    {
        return x.Title == y.Title && x.Artist == y.Artist;
    }

    public int GetHashCode(Record obj)
    {
        return HashCode.Combine(obj.Title, obj.Artist);
    }
}
