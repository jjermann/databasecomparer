using System.Collections.Generic;
using System.Linq;

namespace DatabaseComparer
{
    public class DbStateReference : IDbStateReference
    {
        public DbView DbView { get; }
        public DbState DbState { get; }

        public DbStateReference(DbView dbView, IEnumerable<DbEntry> dbEntries)
        {
            DbView = dbView;
            var dbEntryList = dbEntries.ToList();
            DbState = new DbState
            {
                DbEntryList = dbEntryList
            };
        }

        public IEnumerable<DbBusinessId> GetBusinessIds()
        {
            return DbState.DbEntryList.Select(e => e.BusinessId);
        }

        public IEnumerable<DbEntry> GetDbEntries()
        {
            return DbState.DbEntryList;
        }
    }
}