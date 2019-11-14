using System.Collections.Generic;
using System.Data.SqlClient;

namespace DatabaseComparer
{
    // ReSharper disable once InconsistentNaming
    public class SqlService : ISqlService
    {
        public IEnumerable<DbBusinessId> GetBusinessIds(string connectionString, DbView dbView)
        {
            var query = dbView.GetBusinessIdSelectQuery();
            var businessColumnCount = dbView.BusinessIdColumnNameList.Count;
            var createViewQuery = dbView.CreateViewQuery;

            var cnn = new SqlConnection(connectionString);
            cnn.Open();
            var sqlTransaction = cnn.BeginTransaction();
            try
            {
                var createViewCmd = new SqlCommand(createViewQuery, cnn, sqlTransaction);
                createViewCmd.ExecuteNonQuery();
                var cmd = new SqlCommand(query, cnn, sqlTransaction);
                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var businessIdColumnList = new List<string>();
                        for (var i = 0; i < businessColumnCount; i++)
                        {
                            var businessColumnContent = dataReader.GetValue(i).ToString();
                            businessIdColumnList.Add(businessColumnContent);
                        }

                        var businessId = new DbBusinessId(dbView.ViewName, businessIdColumnList.ToArray());
                        yield return businessId;
                    }
                }
            }
            finally
            {
                sqlTransaction.Rollback();
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
            var sqlTransaction = cnn.BeginTransaction();
            try
            {
                var createViewCmd = new SqlCommand(createViewQuery, cnn, sqlTransaction);
                createViewCmd.ExecuteNonQuery();
                var cmd = new SqlCommand(query, cnn, sqlTransaction);
                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var businessIdColumnList = new List<string>();
                        for (var i = 0; i < businessColumnCount; i++)
                        {
                            var businessColumnContent = dataReader.GetValue(i).ToString();
                            businessIdColumnList.Add(businessColumnContent);
                        }
                        var columnList = new List<string>();
                        for (var i = businessColumnCount; i < businessColumnCount + columnCount; i++)
                        {
                            var columnContent = dataReader.GetValue(i).ToString();
                            columnList.Add(columnContent);
                        }
                        var dbEntry = new DbEntry
                        {
                            BusinessId = new DbBusinessId(dbView.ViewName, businessIdColumnList.ToArray()),
                            ColumnList = columnList
                        };

                        yield return dbEntry;
                    }
                }
            }
            finally
            {
                sqlTransaction.Rollback();
            }

            cnn.Close();
        }
    }
}
