using System.Collections.Generic;

namespace DatabaseComparer
{
    public class SqlDbStateReference : IDbStateReference
    {
        private SqlService _sqlService;
        private string _connectionString;
        public SqlDbStateReference(SqlService sqlService, string connectionString, DbView dbView)
        {
            _sqlService = sqlService;
            _connectionString = connectionString;
            DbView = dbView;
        }

        public DbView DbView { get; }

        public IEnumerable<DbBusinessId> GetBusinessIds()
        {
            return _sqlService.GetBusinessIds(_connectionString, DbView);
        }

        public IEnumerable<DbEntry> GetDbEntries()
        {
            return _sqlService.GetDbEntries(_connectionString, DbView);
        }
    }
}