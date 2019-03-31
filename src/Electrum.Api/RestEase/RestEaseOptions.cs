using System.Collections.Generic;

namespace Electrum.Api.RestEase
{
    public class RestEaseOptions
    {
        public IEnumerable<Service> Services;
    }

    public class Service
    {
        public string Name { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
