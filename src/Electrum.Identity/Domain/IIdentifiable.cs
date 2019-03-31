using System;

namespace Electrum.Identity.Domain
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}
