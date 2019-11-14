using System.Collections.Generic;
using System.Linq;

namespace DatabaseComparer
{
    public class DbState : IDbStateReference
    {
        public DbView DbView { get; }
        public List<DbEntry> DbEntryList {get;set;}

        public DbState(DbView dbView, IEnumerable<DbEntry> dbEntries)
        {
            DbView = dbView;
            DbEntryList = dbEntries.ToList();
        }

        public IEnumerable<DbBusinessId> GetBusinessIds()
        {
            return DbEntryList.Select(e => e.BusinessId);
        }

        public IEnumerable<DbEntry> GetDbEntries()
        {
            return DbEntryList;
        }

        // public override string ToString()
        // {
        //     var dbStateStr = "DbState: " + string.Join(", ", GetDbEntries());
        //     return dbStateStr;
        // }
    }
}