using System;
using DatabaseComparerTests;

namespace TestApp
{
    public static class DbProgram
    {
        static void Main()
        {
            Test();
        }

        private static void Test()
        {
            var sqlService = new SimpleMockSqlService();
            sqlService.Insert(TestDataGenerator.GetTestDbEntryList0().ToArray());
            var stateEnumerable = sqlService.GetDbEntries();
            foreach (var entry in stateEnumerable)
            {
                Console.WriteLine(string.Join(",", entry.BusinessId));
                Console.WriteLine(string.Join(",", entry.ColumnList));
            }
        }
    }
}
