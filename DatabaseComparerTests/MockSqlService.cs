using System.Collections.Generic;
using System.Linq;
using DatabaseComparer;

namespace DatabaseComparerTests
{
    public class MockSqlService : ISqlService
    {
        private readonly Dictionary<(string, string), SimpleMockSqlService> _stateDictionary = new Dictionary<(string, string), SimpleMockSqlService>();

        public int Reset()
        {
            var count = 0;
            foreach (var key in _stateDictionary.Keys)
            {
                count += _stateDictionary[key].Clear();
                _stateDictionary.Remove(key);
            }
            return count;
        }

        public int Clear(string connectionString, string dbViewName)
        {
            if (_stateDictionary.ContainsKey((connectionString, dbViewName)))
            {
                return _stateDictionary[(connectionString, dbViewName)].Clear();
            }
            return 0;
        }

        public int Insert(string connectionString, params DbEntry[] dbEntryArray)
        {
            var viewGroups = dbEntryArray.GroupBy(e => e.BusinessId.ViewName);
            var count = 0;
            foreach (var viewGroup in viewGroups)
            {
                var dbViewName = viewGroup.Key;
                if (!_stateDictionary.ContainsKey((connectionString, dbViewName)))
                {
                    _stateDictionary[(connectionString, dbViewName)] = new SimpleMockSqlService();
                }
                count += _stateDictionary[(connectionString, dbViewName)].Insert(viewGroup.ToArray());
            }
            return count;
        }

        public int Delete(string connectionString, params DbBusinessId[] businessIdArray)
        {
            var viewGroups = businessIdArray.GroupBy(e => e.ViewName);
            var count = 0;
            foreach (var viewGroup in viewGroups)
            {
                var dbViewName = viewGroup.Key;
                if (!_stateDictionary.ContainsKey((connectionString, dbViewName)))
                {
                    _stateDictionary[(connectionString, dbViewName)] = new SimpleMockSqlService();
                }
                count += _stateDictionary[(connectionString, dbViewName)].Delete(viewGroup.ToArray());
            }
            return count;
        }

        public int Update(string connectionString, params DbEntry[] dbEntryArray)
        {
            var viewGroups = dbEntryArray.GroupBy(e => e.BusinessId.ViewName);
            var count = 0;
            foreach (var viewGroup in viewGroups)
            {
                var dbViewName = viewGroup.Key;
                if (!_stateDictionary.ContainsKey((connectionString, dbViewName)))
                {
                    _stateDictionary[(connectionString, dbViewName)] = new SimpleMockSqlService();
                }
                count += _stateDictionary[(connectionString, dbViewName)].Update(viewGroup.ToArray());
            }
            return count;
        }

        public int AddOrUpdate(string connectionString, DbView dbView, params DbEntry[] dbEntryArray)
        {
            var viewGroups = dbEntryArray.GroupBy(e => e.BusinessId.ViewName);
            var count = 0;
            foreach (var viewGroup in viewGroups)
            {
                var dbViewName = viewGroup.Key;
                if (!_stateDictionary.ContainsKey((connectionString, dbViewName)))
                {
                    _stateDictionary[(connectionString, dbViewName)] = new SimpleMockSqlService();
                }
                count += _stateDictionary[(connectionString, dbViewName)].AddOrUpdate(viewGroup.ToArray());
            }
            return count;
        }

        public int ApplyDbDiffEntries(string connectionString, params DbDiffEntry[] dbDiffArray)
        {
            var viewGroups = dbDiffArray.GroupBy(e => e.ViewName);
            var count = 0;
            foreach (var viewGroup in viewGroups)
            {
                var dbViewName = viewGroup.Key;
                if (!_stateDictionary.ContainsKey((connectionString, dbViewName)))
                {
                    _stateDictionary[(connectionString, dbViewName)] = new SimpleMockSqlService();
                }
                count += _stateDictionary[(connectionString, dbViewName)].ApplyDbDiffEntries(viewGroup.ToArray());
            }
            return count;
        }

        public IEnumerable<DbBusinessId> GetBusinessIds(string connectionString, DbView dbView)
        {
            return _stateDictionary[(connectionString, dbView.ViewName)].GetBusinessIds();
        }

        public IEnumerable<DbEntry> GetDbEntries(string connectionString, DbView dbView)
        {
            return _stateDictionary[(connectionString, dbView.ViewName)].GetDbEntries();
        }
    }
}
