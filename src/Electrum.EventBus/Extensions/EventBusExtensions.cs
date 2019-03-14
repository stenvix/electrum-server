using System.Linq;

namespace Electrum.EventBus.Extensions
{
    public static class EventBusExtensions
    {
        public static string Underscore(this string value)
            => string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
    }
}