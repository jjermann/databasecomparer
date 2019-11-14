using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseComparer;

namespace DatabaseComparerTests
{
    public class SimpleMockSqlService : ISqlService
    {
        private readonly List<DbEntry> _dbEntryList = new List<DbEntry>();

        public int Clear()
        {
            var entryCount = _dbEntryList.Count();
            _dbEntryList.Clear();
            return entryCount;
        }

        public int Insert(params DbEntry[] dbEntryArray)
        {
            // Validate
            var existingIdList = dbEntryArray.Select(e => e.BusinessId).Intersect(GetBusinessIds());
            if (existingIdList.Any())
            {
                var msg = "Error during Insert -> The following BusinessIds already exist: " + string.Join(", ", existingIdList);
                throw new Exception(msg);
            }

            _dbEntryList.AddRange(dbEntryArray);
            return dbEntryArray.Count();
        }

        public int Delete(params DbBusinessId[] businessIdArray)
        {
            // Validate
            var nonExistingIdList = businessIdArray.Except(GetBusinessIds());
            if (nonExistingIdList.Any())
            {
                var msg = "Error during Delete -> The following BusinessIds don't exist: " + string.Join(", ", nonExistingIdList);
                throw new Exception(msg);
            }

            var count = _dbEntryList.RemoveAll(e => businessIdArray.Contains(e.BusinessId));
            return count;
        }

        public int Update(params DbEntry[] dbEntryArray)
        {
            // Validate
            var nonExistingIdList = dbEntryArray.Select(e => e.BusinessId).Except(GetBusinessIds());
            if (nonExistingIdList.Any())
            {
                var msg = "Error during Update -> The following BusinessIds don't exist: " + string.Join(", ", nonExistingIdList);
                throw new Exception(msg);
            }

            var changedCount = 0;
            foreach (var dbEntry in dbEntryArray)
            {
                var hasChanged = false;
                var existingDbEntry = _dbEntryList.Single(e => e.BusinessId.Equals(dbEntry.BusinessId));
                for (var i=0; i<dbEntry.ColumnList.Count; i++)
                {
                    if (!hasChanged && existingDbEntry.ColumnList[i] != dbEntry.ColumnList[i])
                    {
                        hasChanged = true;
                        changedCount++;
                    }
                    existingDbEntry.ColumnList[i] = dbEntry.ColumnList[i];
                }
            }
            return changedCount;
        }

        public int AddOrUpdate(params DbEntry[] dbEntryArray)
        {
            var existingBusinessIdList = GetBusinessIds();
            var addArray = dbEntryArray.Where(e => !existingBusinessIdList.Contains(e.BusinessId)).ToArray();
            var updateArray = dbEntryArray.Where(e => existingBusinessIdList.Contains(e.BusinessId)).ToArray();
            var insertCount = Insert(addArray);
            var updateCount = Update(updateArray);
            return insertCount + updateCount;
        }

        public int ApplyDbDiffEntries(params DbDiffEntry[] dbDiffArray)
        {
            var insertArray = dbDiffArray
                .Where(e => e.DiffEntryType == DbDiffEntryType.Add)
                .Select(e => e.DbEntryAfter)
                .ToArray();
            var updateDictionary = dbDiffArray
                .Where(e => e.DiffEntryType == DbDiffEntryType.Update)
                .ToDictionary(e => e.DbEntryBefore.BusinessId, e => e.DbEntryAfter);
            var deleteArray = dbDiffArray
                .Where(e => e.DiffEntryType == DbDiffEntryType.Delete)
                .Select(e => e.DbEntryBefore)
                .ToArray();
            var existingBusinessIdList = GetBusinessIds();

            // Validate
            var invalidInsertList = existingBusinessIdList.Intersect(insertArray.Select(e => e.BusinessId)).ToList();
            if (invalidInsertList.Any())
            {
                var msg = "Error during Insert -> The following BusinessIds already exist: " + string.Join(", ", invalidInsertList);
                throw new Exception(msg);
            }
            var nonExistingIdList = (updateDictionary.Keys.Union(deleteArray.Select(e => e.BusinessId))).Except(existingBusinessIdList).ToList();
            if (nonExistingIdList.Any())
            {
                var msg = "Error during Update/Delete -> The following BusinessIds don't exist: " + string.Join(", ", nonExistingIdList);
                throw new Exception(msg);
            }

            var count = 0;
            count += Insert(insertArray);
            count += Delete(deleteArray.Select(e => e.BusinessId).ToArray());
            count += Update(updateDictionary.Values.ToArray());
            return count;
        }

        public List<DbBusinessId> GetBusinessIds()
        {
            return _dbEntryList.Select(e => e.BusinessId).ToList();
        }

        public List<DbEntry> GetDbEntries()
        {
            return _dbEntryList;
        }

        public IEnumerable<DbBusinessId> GetBusinessIds(string connectionString, DbView dbView)
        {
            return GetBusinessIds();
        }

        public IEnumerable<DbEntry> GetDbEntries(string connectionString, DbView dbView)
        {
            return GetDbEntries();
        }
    }
}
