using System.Collections.Generic;
using System.Data.SqlClient;

namespace DatabaseComparer
{
    public class SqlService : ISqlService
    {
        public IEnumerable<int> GetBusinessIds(string connectionString, DbView dbView)
        {
            var query = dbView.GetBusinessIdSelectQuery();
            var businessColumnCount = dbView.BusinessIdColumnNameList.Count;
            var createViewQuery = dbView.CreateViewQuery;

            var cnn = new SqlConnection(connectionString);
            cnn.Open();
            var createViewCmd = new SqlCommand(createViewQuery, cnn);
            createViewCmd.ExecuteNonQuery();
            var cmd = new SqlCommand(query, cnn);
            using (var dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    var dbEntry = new DbEntry
                    {
                        BusinessIdColumnList = new List<string>(),
                        ColumnList = new List<string>()
                    };
                    for (var i = 0; i < businessColumnCount; i++)
                    {
                        var businessColumnContent = dataReader.GetValue(i).ToString();
                        dbEntry.BusinessIdColumnList.Add(businessColumnContent);
                    }

                    yield return dbEntry.BusinessIdHashCode;
                }
            }
            cnn.Close();
        }

        public IEnumerable<DbEntry> GetDbEntries(string connectionString, DbView dbView)
        {
            var query = dbView.GetFullSelectQuery();
            var businessColumnCount = dbView.BusinessIdColumnNameList.Count;
            var columnCount = dbView.ColumnNameList.Count;
            var createViewQuery = dbView.CreateViewQuery;

            var cnn = new SqlConnection(connectionString);
            cnn.Open();
            var createViewCmd = new SqlCommand(createViewQuery, cnn);
            createViewCmd.ExecuteNonQuery();
            var cmd = new SqlCommand(query, cnn);
            using (var dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    var dbEntry = new DbEntry
                    {
                        BusinessIdColumnList = new List<string>(),
                        ColumnList = new List<string>()
                    };
                    for (var i = 0; i < businessColumnCount; i++)
                    {
                        var businessColumnContent = dataReader.GetValue(i).ToString();
                        dbEntry.BusinessIdColumnList.Add(businessColumnContent);
                    }
                    for (var i = businessColumnCount; i < businessColumnCount + columnCount; i++)
                    {
                        var columnContent = dataReader.GetValue(i).ToString();
                        dbEntry.ColumnList.Add(columnContent);
                    }

                    yield return dbEntry;
                }
            }
            cnn.Close();
        }
    }
}
