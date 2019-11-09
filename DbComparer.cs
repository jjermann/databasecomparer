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
        DbStateReference GetDbState(DbView dbView, DbStateReferenceType refType)
        {
            // TODO
            var query = dbView.GetFullSelectQuery();
            return null;
        }
        DbDiff GetDbDiff(DbStateReference ref1, DbStateReference ref2)
        {
            //TODO
            var idList1=ref1.GetBusinessIdList();
            var idList2=ref2.GetBusinessIdList();
            var addList = idList2.Except(idList1);
            var delList = idList1.Except(idList2);
            var updCandidateList = idList1.Intersect(idList2);
            return null;
        }
        
        DbDiff GetDbDiff(DbDiff diff1, DbDiff diff2)
        {
            var add1Ids = diff1.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Add)
                .Select(e => e.BusinessIdHashCode)
                .ToList();
            var del1Ids = diff1.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Delete)
                .Select(e => e.BusinessIdHashCode)
                .ToList();
            var upd1Ids = diff1.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Update)
                .Select(e => e.BusinessIdHashCode)
                .ToList();
            var add2Ids = diff2.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Add)
                .Select(e => e.BusinessIdHashCode)
                .ToList();
            var del2Ids = diff2.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Delete)
                .Select(e => e.BusinessIdHashCode)
                .ToList();
            var upd2Ids = diff2.DbDiffEntryList
                .Where(e => e.DiffEntryType == DbDiffEntryType.Update)
                .Select(e => e.BusinessIdHashCode)
                .ToList();
            var delBothIds = del1Ids.Intersect(del2Ids).ToList();
            var updBothIds = upd1Ids.Intersect(upd2Ids).ToList();

            // Validate
            var diff1Ids = diff1.DbDiffEntryList.Select(e => e.BusinessIdHashCode).ToList();
            var diff2Ids = diff2.DbDiffEntryList.Select(e => e.BusinessIdHashCode).ToList();
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
            if (delBothIds.Any(id => !diff1.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id).Equals(diff2.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id))))
            {
                throw new ArgumentException("Inconsistent reference for Delete!");
            }
            if (updBothIds.Any(id => !diff1.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id).Equals(diff2.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id))))
            {
                throw new ArgumentException("Inconsistent reference for Update!");
            }

            var dbDiffEntryList = new List<DbDiffEntry>();

            // Add
            var addList1 = add2Ids.Except(add1Ids)
                .Select(id => 
                {
                    var diffEntry = (DbDiffEntry)diff2.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id).Clone();
                    return diffEntry;
                })
                .ToList();
            var addList2 = del1Ids.Except(del2Ids)
                .Select(id => 
                {
                    var dbEntry = (DbEntry)(diff1.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id).DbEntryBefore.Clone());
                    var diffEntry = new DbDiffEntry
                    {
                        DbEntryAfter = dbEntry
                    };
                    return diffEntry;
                })
                .ToList();
            dbDiffEntryList.AddRange(addList1);
            dbDiffEntryList.AddRange(addList2);

            // Delete
            var delList1 = del2Ids.Except(del1Ids)
                .Select(id => 
                {
                    var diffEntry = (DbDiffEntry)diff2.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id).Clone();
                    return diffEntry;
                })
                .ToList();
            var delList2 = add1Ids.Except(add2Ids)
                .Select(id => 
                {
                    var dbEntry = (DbEntry)(diff1.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id).DbEntryBefore.Clone());
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
            foreach (var id in add2Ids.Except(add1Ids))
            {
                var e1 = diff1.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id);
                var e2 = diff2.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id);
                var updateDiffEntry = GetUpdateDifference(e1,e2);
                if (updateDiffEntry != null)
                {
                    updList1.Add(updateDiffEntry);
                }
            }
            var updList2 = upd2Ids.Except(upd1Ids.Union(del1Ids))
                .Select(id =>
                {
                    var diffEntry = (DbDiffEntry)diff2.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id).Clone();
                    return diffEntry;
                })
                .ToList();
            var updList3 = new List<DbDiffEntry>();
            foreach (var id in upd2Ids.Union(upd1Ids))
            {
                var e1 = diff1.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id);
                var e2 = diff2.DbDiffEntryList.Single(e => e.BusinessIdHashCode == id);
                var updateDiffEntry = GetUpdateDifference(e1,e2);
                if (updateDiffEntry != null)
                {
                    updList1.Add(updateDiffEntry);
                }
            }
            dbDiffEntryList.AddRange(updList1);
            dbDiffEntryList.AddRange(updList2);
            dbDiffEntryList.AddRange(updList3);

            // DbDiff
            var dbDiff = new DbDiff
            {
                DbDiffEntryList = dbDiffEntryList.OrderBy(e => e.BusinessIdHashCode).ToList()
            };

            return dbDiff;
        }
        DbDiff SquashDbDiffs(DbDiff[] diffArray) => null;

        private DbDiffEntry GetUpdateDifference(DbDiffEntry e1, DbDiffEntry e2)
        {
            // TODO
            // Case 1: Add,Add
            // Case 2: Upd,Upd
            // return null if there is no difference
            // Else: throw Exception
            return null;
        }
    }
}

