using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseComparer
{
    public class DbBusinessId : IEquatable<DbBusinessId>, IComparable<DbBusinessId>, ICloneable
    {
        public string ViewName { get; }
        public List<string> BusinessIdColumnList { get; }

        public DbBusinessId(string viewName, params string[] columnArray)
        {
            ViewName = viewName;
            BusinessIdColumnList = columnArray.ToList();
        }

        public bool Equals(DbBusinessId other)
        {
            if (other == null)
            {
                return false;
            }
            return GetHashCode().Equals(other.GetHashCode());
        }
        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other.GetType() != GetType())
            {
                return false;
            }
            return Equals(other as DbBusinessId);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ ViewName?.GetHashCode() ?? 0;
                foreach (var column in BusinessIdColumnList)
                {
                    hashCode = (hashCode * 397) ^ column?.GetHashCode() ?? 0;
                }
                return hashCode;
            }
        }
        public static bool operator ==(DbBusinessId lhs, DbBusinessId rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(DbBusinessId lhs, DbBusinessId rhs)
        {
            return !(lhs == rhs);
        }
        public static bool operator <=(DbBusinessId lhs, DbBusinessId rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }
        public static bool operator >=(DbBusinessId lhs, DbBusinessId rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }
        public static bool operator <(DbBusinessId lhs, DbBusinessId rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }
        public static bool operator >(DbBusinessId lhs, DbBusinessId rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public int CompareTo(DbBusinessId other)
        {
            var viewNameCompare = string.Compare(ViewName, other.ViewName, StringComparison.InvariantCulture);
            if (viewNameCompare != 0)
            {
                return viewNameCompare;
            }

            for (var i = 0; i < BusinessIdColumnList.Count; i++)
            {
                var columnCompare = string.Compare(BusinessIdColumnList[i], other.BusinessIdColumnList[i], StringComparison.InvariantCulture);
                if (columnCompare != 0)
                {
                    return columnCompare;
                }
            }

            return 0;
        }

        public object Clone()
        {
            return new DbBusinessId(ViewName, BusinessIdColumnList.ToArray());
        }

        public override string ToString()
        {
            var str = ViewName + "(" + string.Join(",", BusinessIdColumnList) + ")";
            return str;
        }
    }
}
