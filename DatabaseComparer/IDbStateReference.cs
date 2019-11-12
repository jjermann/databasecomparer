using System.Collections.Generic;

namespace DatabaseComparer
{
    public interface IDbStateReference
    {
        DbView DbView {get;}
        IEnumerable<DbEntry> GetDbEntries();
        IEnumerable<int> GetBusinessIds();
    }
}