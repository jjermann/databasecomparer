using System;

namespace DatabaseComparer
{
    public class DbDiffEntry : ICloneable, IEquatable<DbDiffEntry>, IComparable<DbDiffEntry>
    {
        public DbEntry DbEntryBefore {get;set;}
        public DbEntry DbEntryAfter {get;set;}

        public string ViewName => DbEntryBefore?.ViewName ?? DbEntryAfter?.ViewName;
        public int BusinessIdHashCode => (DbEntryBefore?.BusinessIdHashCode ?? DbEntryAfter?.BusinessIdHashCode).Value;
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
            // TODO
            // if (BusinessIdHashCode != other.BusinessIdHashCode)
            // {
            //     for (var i=0; i<BusinessIdColumnList.Count; i++)
            //     {
            //         var businessIdColumnCompare = BusinessIdColumnList[i].Value.CompareTo(other.BusinessIdColumnList[i].Value);
            //         if (businessIdColumnCompare != 0)
            //         {
            //             return businessIdColumnCompare;
            //         }
            //     }
            //     throw new Exception("Inconsistency!");
            // }
            // for (var i=0; i<ColumnList.Count; i++)
            // {
            //     var columnCompare = ColumnList[i].Value.CompareTo(other.ColumnList[i].Value);
            //     if (columnCompare != 0)
            //     {
            //         return columnCompare;
            //     }
            // }
            return 0;
        }
        public bool Equals(DbDiffEntry other)
        {
            if (other == null) {
                return false;
            }
            // TODO: First check BusinessIdHashCode
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
    }
}
