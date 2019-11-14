using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseComparer
{
    public class DbEntry : ICloneable, IEquatable<DbEntry>, IComparable<DbEntry>
    {
        public DbBusinessId BusinessId {get;set;}
        public List<string> ColumnList {get;set;}

        public int CompareTo(DbEntry other)
        {
            var businessIdCompare = BusinessId.CompareTo(other.BusinessId);
            if (businessIdCompare != 0)
            {
                return businessIdCompare;
            }
            for (var i=0; i<ColumnList.Count; i++)
            {
                var columnCompare = string.Compare(ColumnList[i], other.ColumnList[i], StringComparison.InvariantCulture);
                if (columnCompare != 0)
                {
                    return columnCompare;
                }
            }
            return 0;
        }
        public bool Equals(DbEntry other)
        {
            if (other == null) {
                return false;
            }

            if (BusinessId.GetHashCode() != other.BusinessId.GetHashCode())
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
            return Equals(other as DbEntry);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                hashCode = (hashCode * 397) ^ BusinessId.GetHashCode();
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                foreach (var column in ColumnList)
                {
                    hashCode = (hashCode * 397) ^ column?.GetHashCode() ?? 0;
                }
                return hashCode;
            }
        }
        public static bool operator ==(DbEntry lhs, DbEntry rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(DbEntry lhs, DbEntry rhs)
        {
            return !(lhs == rhs);
        }
        public static bool operator <=(DbEntry lhs, DbEntry rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }
        public static bool operator >=(DbEntry lhs, DbEntry rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }
        public static bool operator <(DbEntry lhs, DbEntry rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }
        public static bool operator >(DbEntry lhs, DbEntry rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public object Clone()
        {
            return new DbEntry
            {
                BusinessId = (DbBusinessId)BusinessId.Clone(),
                ColumnList = ColumnList?.Select(p => p).ToList(),
            };
        }

        public override string ToString()
        {
            var str = BusinessId + " " + string.Join(",", ColumnList);
            return str;
        }
    }
}
