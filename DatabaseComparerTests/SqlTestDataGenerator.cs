using System.Collections.Generic;
using DatabaseComparer;

namespace DatabaseComparerTests
{
    public static class SqlTestDataGenerator
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
    }
}
