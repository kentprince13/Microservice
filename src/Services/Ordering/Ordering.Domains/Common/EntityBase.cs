using System;

namespace Ordering.Domains.Common
{
    public abstract class EntityBase
    {
        public long Id { get; protected set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
