using System;

namespace Electrum.Common.Domain
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}
