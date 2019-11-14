using System.Collections.Generic;

namespace DatabaseComparer
{
    public interface ISqlService
    {
        IEnumerable<DbBusinessId> GetBusinessIds(string connectionString, DbView dbView);
        IEnumerable<DbEntry> GetDbEntries(string connectionString, DbView dbView);
    }
}
