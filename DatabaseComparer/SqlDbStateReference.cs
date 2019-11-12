using System.Collections.Generic;

namespace DatabaseComparer
{
    public class SqlDbStateReference : IDbStateReference
    {
        private SqlService _sqlService;
        private string _connectionString;
        public SqlDbStateReference(SqlService sqlService, string connectionSstring, DbView dbView)
        {
            _sqlService = sqlService;
            _connectionString = connectionSstring;
            DbView = dbView;
        }

        public DbView DbView { get; }

        public IEnumerable<int> GetBusinessIds()
        {
            return _sqlService.GetBusinessIds(_connectionString, DbView);
        }

        public IEnumerable<DbEntry> GetDbEntries()
        {
            return _sqlService.GetDbEntries(_connectionString, DbView);
        }
    }
}