using System.Collections.Generic;
using System.Linq;

namespace DatabaseComparer
{
    public class DbStateReference : IDbStateReference
    {
        private DbView _dbView;
        private DbState _dbState;

        public DbStateReference(DbView dbView, IEnumerable<DbEntry> dbEntries)
        {
            _dbView = dbView;
            var dbEntryList = dbEntries.ToList();
            _dbState = new DbState
            {
                DbEntryList = dbEntryList
            };
        }

        public DbView DbView { get; }

        public IEnumerable<int> GetBusinessIds()
        {
            return _dbState.DbEntryList.Select(e => e.BusinessIdHashCode);
        }

        public IEnumerable<DbEntry> GetDbEntries()
        {
            return _dbState.DbEntryList;
        }
    }
}