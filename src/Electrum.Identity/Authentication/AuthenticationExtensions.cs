using Electrum.Common.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Electrum.Identity.Authentication
{
    public static class AuthenticationExtensions
    {
        private static readonly string JwtSectionName = "jwt";
        public static void AddJwt(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }
            var section = configuration.GetSection(JwtSectionName);
            services.Configure<JwtOptions>(section);
            services.AddSingleton<IJwtHandler, JwtHandler>();
        }
    }
}
