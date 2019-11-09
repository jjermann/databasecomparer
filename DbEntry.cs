using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseComparer
{
    public class DbEntry : ICloneable, IEquatable<DbEntry>, IComparable<DbEntry>
    {
        public string ViewName {get;set;}
        public List<string> BusinessIdColumnList {get;set;}
        public List<string> ColumnList {get;set;}
        public int BusinessIdHashCode => 0;

        public int CompareTo(DbEntry other)
        {
            var viewNameCompare = ViewName.CompareTo(other.ViewName);
            if (viewNameCompare != 0)
            {
                return viewNameCompare;
            }
            if (BusinessIdHashCode != other.BusinessIdHashCode)
            {
                for (var i=0; i<BusinessIdColumnList.Count; i++)
                {
                    var businessIdColumnCompare = BusinessIdColumnList[i].CompareTo(other.BusinessIdColumnList[i]);
                    if (businessIdColumnCompare != 0)
                    {
                        return businessIdColumnCompare;
                    }
                }
                throw new Exception("Inconsistency!");
            }
            for (var i=0; i<ColumnList.Count; i++)
            {
                var columnCompare = ColumnList[i].CompareTo(other.ColumnList[i]);
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
            return Equals(other as DbEntry);
        }
        public override int GetHashCode()
        {
           	unchecked
            {
                var hashCode = 13;
                // TODO
                // hashCode = (hashCode * 397) ^ ViewName.GetHashCode();
                // hashCode = (hashCode * 397) ^ BusinessIdColumnList.GetHashCode();
                // hashCode = (hashCode * 397) ^ ColumnList.GetHashCode();
                return hashCode;
            }
        }
        public static bool operator ==(DbEntry lhs, DbEntry rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
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
                ViewName = ViewName,
                BusinessIdColumnList = BusinessIdColumnList?.Select(p => p).ToList(),
                ColumnList = ColumnList?.Select(p => p).ToList(),
            };
        }
    }
}
