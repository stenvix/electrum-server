using Electrum.Api.RestEase;
using Electrum.Common.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RestEase;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Electrum.Api
{
    public static class Extensions
    {
        private static readonly string JwtSectionName = "jwt";

        public static void RegisterServiceForwarder<T>(this IServiceCollection services, string serviceName) where T : class
        {
            var clientName = typeof(T).ToString();
            var options = ConfigureOptions(services);
            ConfigureDefaultClient(services, clientName, serviceName, options);
            ConfigureForwarder<T>(services, clientName);
        }

        public static void AddJwt(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }
            //TODO: Find out if this code is used
            var section = configuration.GetSection(JwtSectionName);
            var options = section.Get<JwtOptions>();
            services.Configure<JwtOptions>(section);
            services.AddSingleton(options);
            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                        ValidIssuer = options.Issuer,
                        ValidAudience = options.ValidAudience,
                        ValidateAudience = options.ValidateAudience,
                        ValidateLifetime = options.ValidateLifetime
                    };
                });
        }

        private static void ConfigureForwarder<T>(IServiceCollection services, string clientName) where T : class
        {
            services.AddTransient<T>(c => new RestClient(c.GetService<IHttpClientFactory>().CreateClient(clientName))
            {
                RequestQueryParamSerializer = new QueryParamSerializer()
            }.For<T>());
        }

        private static void ConfigureDefaultClient(IServiceCollection services, string clientName, string serviceName, RestEaseOptions options)
        {
            services.AddHttpClient(clientName, client =>
            {
                var service = options.Services.SingleOrDefault(i =>
                    i.Name.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase));
                if (service == null)
                {
                    throw new RestEaseServiceNotFoundException($"RestEase service: '{serviceName}' was not found.", serviceName);
                }

                client.BaseAddress = new UriBuilder
                {
                    Scheme = service.Scheme,
                    Host = service.Host,
                    Port = service.Port
                }.Uri;
            });
        }

        private static RestEaseOptions ConfigureOptions(IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.Configure<RestEaseOptions>(configuration.GetSection("restEase"));

            return configuration.GetSection("restEase").Get<RestEaseOptions>();
        }
    }
}
