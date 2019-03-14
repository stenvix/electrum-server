using System;

namespace Electrum.Data.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
