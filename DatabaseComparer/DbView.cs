using System.Collections.Generic;
using System.Text;

namespace DatabaseComparer
{
    public class DbView
    {
        public string DbName { get; set; } = null!;
        public DbBusinessId BusinessNameId {get;set;} = null!;
        public string CreateViewQuery {get;set;} = null!;
        public List<string> ColumnNameList {get;set;} = null!;

        public string ViewName => BusinessNameId.ViewName;
        public List<string> BusinessIdColumnNameList => BusinessNameId.BusinessIdColumnList;

        public string GetBusinessIdSelectQuery()
        {
            var businessIdColumnNameString = string.Join(", ", BusinessIdColumnNameList);
            var sb = new StringBuilder();
            sb.AppendLine($"SELECT {businessIdColumnNameString}");
            sb.AppendLine($"FROM {ViewName}");
            return sb.ToString();
        }

        public string GetBasicSelectQuery()
        {
            var columnNameList = new List<string>();
            columnNameList.AddRange(BusinessIdColumnNameList);
            columnNameList.AddRange(ColumnNameList);
            var allColumnNameString = string.Join(", ", columnNameList);
            var sb = new StringBuilder();
            sb.AppendLine($"SELECT {allColumnNameString}");
            sb.AppendLine($"FROM {ViewName}");
            return sb.ToString();
        }

        public string GetFullSelectQuery()
        {
            var businessIdColumnNameString = string.Join(", ", BusinessIdColumnNameList);
            var sb = new StringBuilder();
            sb.AppendLine(GetBasicSelectQuery());
            sb.AppendLine($"ORDER BY {businessIdColumnNameString}");
            return sb.ToString();
        }

        public override string ToString()
        {
            var str = BusinessNameId + " " + string.Join(",", ColumnNameList);
            return str;
        }
    }
}