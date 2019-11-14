using System.Collections.Generic;
using System.Linq;
using DatabaseComparer;

namespace TestApp
{
    public class MockSqlService : ISqlService
    {
        private readonly List<DbEntry> _dbEntryList;
        public MockSqlService(List<DbEntry> dbEntryList)
        {
            _dbEntryList = dbEntryList;
        }

        public IEnumerable<DbBusinessId> GetBusinessIds(string connectionString, DbView dbView)
        {
            return _dbEntryList.Select(e => e.BusinessId);
        }

        public IEnumerable<DbEntry> GetDbEntries(string connectionString, DbView dbView)
        {
            return _dbEntryList;
        }
    }
}
