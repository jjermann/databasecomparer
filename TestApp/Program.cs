using System;
using System.Collections.Generic;
using DatabaseComparer;

namespace TestApp
{
    public static class DbProgram
    {
        static void Main()
        {
            var dbView = new DbView
            {
                BusinessIdColumnNameList = new List<string>
                {
                    "MetadataId"
                },
                ColumnNameList = new List<string>
                {
                    "Adressnummer",
                    "Folgenummer"
                },
                ViewName = "MetadataIdView",
                CreateViewQuery = "CREATE VIEW MetadataIdView AS SELECT MetadataId, Adressnummer, Folgenummer FROM DX_Metadata"
            };
            var dbEntryList = new List<DbEntry>
            {
                new DbEntry
                {
                    BusinessId = new DbBusinessId("MetadataIdView", "1"),
                    ColumnList = new List<string> {"11111111", "1"}
                },
                new DbEntry
                {
                    BusinessId = new DbBusinessId("MetadataIdView", "2"),
                    ColumnList = new List<string> {"11111111", "2"}
                },
                new DbEntry
                {
                    BusinessId = new DbBusinessId("MetadataIdView", "3"),
                    ColumnList = new List<string> {"22222222", "1"}
                }
            };
            var sqlService = new MockSqlService(dbEntryList);
            //var sqlService = new SqlService();
            var connectionString = @"Data Source={host}\{instance}; Initial Catalog={dbName}; User ID={userId}; Password={password}";
            var stateEnumerable = sqlService.GetDbEntries(connectionString, dbView);
            foreach (var entry in stateEnumerable)
            {
                Console.WriteLine(string.Join(",", entry.BusinessId));
                Console.WriteLine(string.Join(",", entry.ColumnList));
            }
        }
    }
}
