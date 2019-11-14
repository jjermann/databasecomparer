using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseComparer
{
    public class DbDiff : ICloneable, IEquatable<DbDiff>
    {
        public List<DbDiffEntry> DbDiffEntryList {get;set;}

        public bool Equals(DbDiff other)
        {
            if (other == null) {
                return false;
            }
            if (DbDiffEntryList == null && other.DbDiffEntryList != null)
            {
                return false;
            }
            if (DbDiffEntryList != null && other.DbDiffEntryList == null)
            {
                return false;
            }
            if (DbDiffEntryList.Count != other.DbDiffEntryList.Count)
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
            return Equals(other as DbDiff);
        }
        public override int GetHashCode()
        {
           	unchecked
            {
                var hashCode = 13;
                if (DbDiffEntryList != null)
                {
                    foreach (var dbDiffEntry in DbDiffEntryList)
                    {
                        hashCode = (hashCode * 397) ^ dbDiffEntry?.GetHashCode() ?? 0;
                    }
                }
                return hashCode;
            }
        }
        public static bool operator ==(DbDiff lhs, DbDiff rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        public static bool operator !=(DbDiff lhs, DbDiff rhs)
        {
            return !(lhs == rhs);
        }

        public object Clone()
        {
            return new DbDiff
            {
                DbDiffEntryList = DbDiffEntryList?.Select(e => (DbDiffEntry)e?.Clone()).ToList()
            };
        }

        public override string ToString()
        {
            var dbDiffStr = "DbDiff: " + string.Join(", ", DbDiffEntryList);
            return dbDiffStr;
        }
    }
}
