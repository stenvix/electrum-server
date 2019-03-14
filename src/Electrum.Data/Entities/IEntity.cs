using System;

namespace Electrum.Data.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
        DateTime? Updated { get; set; }
    }
}
