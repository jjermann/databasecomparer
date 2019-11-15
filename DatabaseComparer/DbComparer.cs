using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseComparer
{
    public class DbComparer
    {
        bool IsValid(DbView dbView)
        {
            // TODO
            var query = dbView.CreateViewQuery;
            return false;
        }
        public SqlDbStateReference GetSqlDbStateReference(string connectionString, DbView dbView)
        {
            var stateReference = new SqlDbStateReference(new SqlService(), connectionString, dbView);
            return stateReference;
        }

        public DbState GetDbState(SqlDbStateReference sqlDbStateReference)
        {
            var dbView = sqlDbStateReference.DbView;
            var dbEntries = sqlDbStateReference.GetDbEntries();
            var stateReference = new DbState(dbView, dbEntries);
            return stateReference;
        }

        public DbDiff GetDbDiff(IDbStateReference ref1, IDbStateReference ref2)
        {
            // TODO: Heavily improve performance/memory usage!!!
            var idList1 = ref1.GetBusinessIds().ToList();
            var idList2 = ref2.GetBusinessIds().ToList();
            var addIdList = idList2.Except(idList1);
            var delIdList = idList1.Except(idList2);
            var updIdCandidateList = idList1.Intersect(idList2);

            var diffEntryList = new List<DbDiffEntry>();
            var addList = ref2.GetDbEntries()
                .Where(e => addIdList.Contains(e.BusinessId))
                .Select(e => new DbDiffEntry
                {
                    DbEntryAfter = e
                })
                .ToList();
            diffEntryList.AddRange(addList);
            var delList = ref1.GetDbEntries()
                .Where(e => delIdList.Contains(e.BusinessId))
                .Select(e => new DbDiffEntry
                {
                    DbEntryBefore = e
                })
                .ToList();
            diffEntryList.AddRange(delList);
            foreach (var updIdCandidate in updIdCandidateList)
            {
                var entry1 = ref1.GetDbEntries().Single(e => e.BusinessId == updIdCandidate);
                var entry2 = ref2.GetDbEntries().Single(e => e.BusinessId == updIdCandidate);
                if (!entry1.Equals(entry2))
                {
                    var dbDiffEntry = new DbDiffEntry
                    {
                        DbEntryBefore = entry1,
                        DbEntryAfter = entry2
                    };
                    diffEntryList.Add(dbDiffEntry);
                }
            }
            diffEntryList.Sort();
            return new DbDiff
            {
                DbDiffEntryList = diffEntryList
            };
        }
        
        public DbDiff GetDbDiff(DbDiff diff1, DbDiff diff2)
        {
            var add1Ids = diff1.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Add)
                .Select(e => e.GetBusinessIdHashCode())
                .ToList();
            var del1Ids = diff1.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Delete)
                .Select(e => e.GetBusinessIdHashCode())
                .ToList();
            var upd1Ids = diff1.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Update)
                .Select(e => e.GetBusinessIdHashCode())
                .ToList();
            var add2Ids = diff2.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Add)
                .Select(e => e.GetBusinessIdHashCode())
                .ToList();
            var del2Ids = diff2.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Delete)
                .Select(e => e.GetBusinessIdHashCode())
                .ToList();
            var upd2Ids = diff2.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Update)
                .Select(e => e.GetBusinessIdHashCode())
                .ToList();
            var delBothIds = del1Ids.Intersect(del2Ids).ToList();
            var updBothIds = upd1Ids.Intersect(upd2Ids).ToList();

            // Validate
            var diff1Ids = diff1.DbDiffEntryList.Select(e => e.GetBusinessIdHashCode()).ToList();
            var diff2Ids = diff2.DbDiffEntryList.Select(e => e.GetBusinessIdHashCode()).ToList();
            if (diff1Ids.Distinct().Count() != diff1Ids.Count())
            {
                throw new ArgumentException("Non-distinct business ids for diff1!");
            }
            if (diff2Ids.Distinct().Count() != diff2Ids.Count())
            {
                throw new ArgumentException("Non-distinct business ids for diff2!");
            }
            if (add1Ids.Intersect(del2Ids).Any() || add2Ids.Intersect(del1Ids).Any())
            {
                throw new ArgumentException("Inconsistency: Can't Add and Delete!");
            }
            if (add1Ids.Intersect(upd2Ids).Any() || add2Ids.Intersect(upd1Ids).Any())
            {
                throw new ArgumentException("Inconsistency: Can't Add and Update!");
            }
            if (delBothIds.Any(id => !diff1.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id).Equals(diff2.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id))))
            {
                throw new ArgumentException("Inconsistent reference for Delete!");
            }
            if (updBothIds.Any(id => !diff1.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id).Equals(diff2.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id))))
            {
                throw new ArgumentException("Inconsistent reference for Update!");
            }

            var dbDiffEntryList = new List<DbDiffEntry>();

            // Add
            var addList1 = add2Ids.Except(add1Ids)
                .Select(id => 
                {
                    var diffEntry = (DbDiffEntry)diff2.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id).Clone();
                    return diffEntry;
                })
                .ToList();
            var addList2 = del1Ids.Except(del2Ids).Except(upd2Ids)
                .Select(id => 
                {
                    var dbEntry = (DbEntry)(diff1.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id).DbEntryBefore.Clone());
                    var diffEntry = new DbDiffEntry
                    {
                        DbEntryAfter = dbEntry
                    };
                    return diffEntry;
                })
                .ToList();
            var addList3 = (del1Ids.Except(del2Ids)).Intersect(upd2Ids)
                .Select(id => 
                {
                    var dbEntry = (DbEntry)(diff2.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id).DbEntryAfter.Clone());
                    var diffEntry = new DbDiffEntry
                    {
                        DbEntryAfter = dbEntry
                    };
                    return diffEntry;
                })
                .ToList();
            dbDiffEntryList.AddRange(addList1);
            dbDiffEntryList.AddRange(addList2);
            dbDiffEntryList.AddRange(addList3);

            // Delete
            var delList1 = del2Ids.Except(del1Ids)
                .Select(id => 
                {
                    var diffEntry = (DbDiffEntry)diff2.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id).Clone();
                    return diffEntry;
                })
                .ToList();
            var delList2 = add1Ids.Except(add2Ids)
                .Select(id => 
                {
                    var dbEntry = (DbEntry)(diff1.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id).DbEntryAfter.Clone());
                    var diffEntry = new DbDiffEntry
                    {
                        DbEntryBefore = dbEntry
                    };
                    return diffEntry;
                })
                .ToList();
            dbDiffEntryList.AddRange(delList1);
            dbDiffEntryList.AddRange(delList2);

            // Update
            var updList1 = new List<DbDiffEntry>();
            foreach (var id in add2Ids.Intersect(add1Ids))
            {
                var e1 = diff1.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id);
                var e2 = diff2.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id);
                var updateDiffEntry = GetUpdateDifference(e1,e2);
                if (updateDiffEntry != null)
                {
                    updList1.Add(updateDiffEntry);
                }
            }
            var updList2 = upd2Ids.Except(upd1Ids.Union(del1Ids))
                .Select(id =>
                {
                    var diffEntry = (DbDiffEntry)diff2.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id).Clone();
                    return diffEntry;
                })
                .ToList();
            var updList3 = new List<DbDiffEntry>();
            foreach (var id in upd2Ids.Union(upd1Ids).Except(del1Ids.Union(del2Ids)))
            {
                var e1 = diff1.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id);
                var e2 = diff2.DbDiffEntryList.Single(e => e.GetBusinessIdHashCode() == id);
                var updateDiffEntry = GetUpdateDifference(e1,e2);
                if (updateDiffEntry != null)
                {
                    updList1.Add(updateDiffEntry);
                }
            }
            dbDiffEntryList.AddRange(updList1);
            dbDiffEntryList.AddRange(updList2);
            dbDiffEntryList.AddRange(updList3);
            dbDiffEntryList.Sort();

            // DbDiff
            var dbDiff = new DbDiff
            {
                DbDiffEntryList = dbDiffEntryList
            };

            return dbDiff;
        }
        // TODO
        public DbDiff SquashDbDiffs(DbDiff[] diffArray) => null;

        // This helper method assumes that both entries have the same BusinessId
        // and that they are of type (Add, Add) or (Update, Update).
        // The return value is the difference between the two DbDiffEntry and null if they are the same.
        private DbDiffEntry GetUpdateDifference(DbDiffEntry e1, DbDiffEntry e2)
        {
            var isAddAdd = e1.DiffEntryType == DbDiffEntryType.Add && e2.DiffEntryType == DbDiffEntryType.Add;
            var isUpdUpd = e1.DiffEntryType == DbDiffEntryType.Update && e2.DiffEntryType == DbDiffEntryType.Update;
            if (isAddAdd || isUpdUpd)
            {
                var entry1Result = e1.DbEntryAfter;
                var entry2Result = e2.DbEntryAfter;
                if (entry1Result.Equals(entry2Result))
                {
                    return null;
                }
                var dbDiffEntry = new DbDiffEntry
                {
                    DbEntryBefore = entry1Result,
                    DbEntryAfter = entry2Result
                };
                return dbDiffEntry;
            }
            throw new NotImplementedException();
        }
    }
}

