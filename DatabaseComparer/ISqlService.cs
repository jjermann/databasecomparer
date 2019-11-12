using System.Collections.Generic;

namespace DatabaseComparer
{
    public interface ISqlService
    {
        IEnumerable<int> GetBusinessIds(string connectionString, DbView dbView);
        IEnumerable<DbEntry> GetDbEntries(string connectionString, DbView dbView);
    }
}
