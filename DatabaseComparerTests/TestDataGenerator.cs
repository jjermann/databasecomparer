using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseComparer;

namespace DatabaseComparerTests
{
    public static class TestDataGenerator
    {
        public const string TestDbViewName = "TestDbView";
        public const string TestDbTableName = "TestDbTable";

        public static List<DbEntry> GetTestDbEntryList0()
        {
            var dbEntryList = new List<DbEntry>
            {
                new DbEntry
                {
                    BusinessId = new DbBusinessId(TestDbViewName, "1", "A"),
                    ColumnList = new List<string> {"111", "1", "B"}
                },
                new DbEntry
                {
                    BusinessId = new DbBusinessId(TestDbViewName, "1", "B"),
                    ColumnList = new List<string> {"111", "2", "A"}
                },
                new DbEntry
                {
                    BusinessId = new DbBusinessId(TestDbViewName, "2", "A"),
                    ColumnList = new List<string> {"222", "1", "A"}
                }
            };
            return dbEntryList;
        }

        public static List<DbEntry> GetTestDbEntryList1()
        {
            var dbEntryList = new List<DbEntry>
            {
                new DbEntry
                {
                    BusinessId = new DbBusinessId(TestDbViewName, "1", "A"),
                    ColumnList = new List<string> {"111", "1", "A"}
                },
                new DbEntry
                {
                    BusinessId = new DbBusinessId(TestDbViewName, "1", "B"),
                    ColumnList = new List<string> {"111", "2", "A"}
                },
                new DbEntry
                {
                    BusinessId = new DbBusinessId(TestDbViewName, "2", "B"),
                    ColumnList = new List<string> {"222", "2", "A"}
                },
            };
            return dbEntryList;
        }

        public static List<DbEntry> GetTestDbEntryList2()
        {
            var dbEntryList = new List<DbEntry>
            {
                new DbEntry
                {
                    BusinessId = new DbBusinessId(TestDbViewName, "1", "A"),
                    ColumnList = new List<string> {"111", "1", "A"}
                },
                new DbEntry
                {
                    BusinessId = new DbBusinessId(TestDbViewName, "2", "A"),
                    ColumnList = new List<string> {"222", "1", "B"}
                },
                new DbEntry
                {
                    BusinessId = new DbBusinessId(TestDbViewName, "2", "B"),
                    ColumnList = new List<string> {"222", "2", "A"}
                },
            };
            return dbEntryList;
        }

        private static List<DbEntry> GetTestDbEntryRandomList(int n, int idNum)
        {
            var columnNum = (int)Math.Floor(Math.Sqrt(n));
            var idList = GetBusinessIdList(idNum);
            var columnListList = GetColumnList(columnNum);
            var rnd = new Random();
            var dbEntryList = new List<DbEntry>();
            while (idList.Any() && dbEntryList.Count < n)
            {
                var rndIdIndex = rnd.Next(idList.Count);
                var businessId = idList[rndIdIndex];
                idList.RemoveAt(rndIdIndex);
                var rndIndex = rnd.Next(columnListList.Count);
                var columnList = columnListList[rndIndex].ToList();
                var dbEntry = new DbEntry
                {
                    BusinessId = businessId,
                    ColumnList = columnList
                };
                dbEntryList.Add(dbEntry);
            }
            return dbEntryList;
        }

        private static List<DbBusinessId> GetBusinessIdList(int n)
        {
            var businessIdList = new List<DbBusinessId>();
            var num = (int)Math.Floor(Math.Sqrt(n));
            for (var i=0; i<num; i++)
            {
                for (var j=0; j<num && businessIdList.Count < n; j++)
                {
                    businessIdList.Add(new DbBusinessId(TestDbViewName, i.ToString(), j.ToString()));
                }
            }
            return businessIdList;
        }

        private static List<List<string>> GetColumnList(int n)
        {
            var columnListList = new List<List<string>>();
            var num = (int)Math.Floor(Math.Pow(n,1.0/3));
            for (var i=0; i<num; i++)
            {
                for (var j=0; j<num; j++)
                {
                    for (var k=0; k<num && columnListList.Count < n ; k++)
                    {
                        var columnList = new List<string> {i.ToString(), j.ToString(), k.ToString()};
                        columnListList.Add(columnList);
                    }
                }
            }
            return columnListList;
        }

        public static DbView GetTestDbView()
        {
            return new DbView
            {
                BusinessNameId = new DbBusinessId(TestDbViewName, "Id1", "Id2"),
                ColumnNameList = new List<string> { "Column1", "Column2", "Column3" },
                CreateViewQuery = $"CREATE VIEW {TestDbViewName} AS SELECT Id1, Id2, Column1, Column2, Column3 FROM {TestDbTableName}"
            };
        }

        public static DbState GetTestDbState0() => new DbState(GetTestDbView(), GetTestDbEntryList0().ToArray());
        public static DbState GetTestDbState1() => new DbState(GetTestDbView(), GetTestDbEntryList1().ToArray());
        public static DbState GetTestDbState2() => new DbState(GetTestDbView(), GetTestDbEntryList2().ToArray());

        public static DbState GetTestDbRandomState(int n)
        {
            var dbEntryList = GetTestDbEntryRandomList(n, 2*n);
            var dbState = new DbState(GetTestDbView(), dbEntryList);
            return dbState;
        }
    }
}
