using System;

namespace DatabaseComparer
{
    public class DbDiffEntry : ICloneable, IEquatable<DbDiffEntry>, IComparable<DbDiffEntry>
    {
        public DbEntry DbEntryBefore {get;set;}
        public DbEntry DbEntryAfter {get;set;}

        public string ViewName => DbEntryBefore?.BusinessId?.ViewName ?? DbEntryAfter?.BusinessId?.ViewName;
        public DbBusinessId BusinessId => DbEntryBefore?.BusinessId ?? DbEntryAfter?.BusinessId;
        public int GetBusinessIdHashCode() => BusinessId?.GetHashCode() ?? 0;
        public DbDiffEntryType DiffEntryType 
        {
            get
            {
                if (DbEntryBefore == null)
                {
                    return DbDiffEntryType.Add;
                }
                if (DbEntryAfter == null)
                {
                    return DbDiffEntryType.Delete;
                }
                return DbDiffEntryType.Update;
            }
        }

        public int CompareTo(DbDiffEntry other)
        {
            var viewNameCompare = ViewName.CompareTo(other.ViewName);
            if (viewNameCompare != 0)
            {
                return viewNameCompare;
            }
            var businessIdCompare = GetBusinessIdHashCode().CompareTo(other.GetBusinessIdHashCode());
            if (businessIdCompare != 0)
            {
                return businessIdCompare;
            }
            var typeCompare = ((int)DiffEntryType).CompareTo((int)(other.DiffEntryType));
            if (typeCompare != 0)
            {
                return typeCompare;
            }
            if (DbEntryBefore != null)
            {
                var beforeCompare = DbEntryBefore.CompareTo(other.DbEntryBefore);
                if (beforeCompare != 0)
                {
                    return beforeCompare;
                }
            }
            if (DbEntryAfter != null)
            {
                var afterCompare = DbEntryAfter.CompareTo(other.DbEntryAfter);
                if (afterCompare != 0)
                {
                    return afterCompare;
                }
            }
            return 0;
        }
        public bool Equals(DbDiffEntry other)
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
            return Equals(other as DbDiffEntry);
        }
        public override int GetHashCode()
        {
           	unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ (DbEntryBefore != null ? DbEntryBefore.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DbEntryAfter != null ? DbEntryAfter.GetHashCode() : 0);
                return hashCode;
            }
        }
        public static bool operator ==(DbDiffEntry lhs, DbDiffEntry rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(DbDiffEntry lhs, DbDiffEntry rhs)
        {
            return !(lhs == rhs);
        }
        public static bool operator <=(DbDiffEntry lhs, DbDiffEntry rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }
        public static bool operator >=(DbDiffEntry lhs, DbDiffEntry rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }
        public static bool operator <(DbDiffEntry lhs, DbDiffEntry rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }
        public static bool operator >(DbDiffEntry lhs, DbDiffEntry rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public object Clone()
        {
            return new DbDiffEntry
            {
                DbEntryBefore = (DbEntry)DbEntryBefore?.Clone(),
                DbEntryAfter = (DbEntry)DbEntryAfter?.Clone()
            };
        }

        public override string ToString()
        {
            var businessId = DbEntryBefore?.BusinessId ?? DbEntryAfter?.BusinessId;
            var str = DiffEntryType + " " + businessId + " ";
            if (DiffEntryType == DbDiffEntryType.Add)
            {
                str += string.Join(",", DbEntryAfter.ColumnList);
            }
            else if (DiffEntryType == DbDiffEntryType.Delete)
            {
                str += string.Join(",", DbEntryBefore.ColumnList);
            }
            else if (DiffEntryType == DbDiffEntryType.Update)
            {
                str += string.Join(",", DbEntryBefore.ColumnList);
                str += " -> ";
                str += string.Join(",", DbEntryAfter.ColumnList);
            }
            return str;
        }
    }
}
