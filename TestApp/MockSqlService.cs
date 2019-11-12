using System.Collections.Generic;
using System.Linq;

namespace DatabaseComparer
{
    public class MockSqlService : ISqlService
    {
        private List<DbEntry> _dbEntryList;
        public MockSqlService(List<DbEntry> dbEntryList)
        {
            _dbEntryList = dbEntryList;
        }

        public IEnumerable<int> GetBusinessIds(string connectionString, DbView dbView)
        {
            return _dbEntryList.Select(e => e.BusinessIdHashCode);
        }

        public IEnumerable<DbEntry> GetDbEntries(string connectionString, DbView dbView)
        {
            return _dbEntryList;
        }
    }
}
