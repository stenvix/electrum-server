using Autofac;
using Autofac.Extensions.DependencyInjection;
using Electrum.Common.Mongo;
using Electrum.EventBusRabbitMQ;
using Electrum.Identity.Authentication;
using Electrum.Identity.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Electrum.Identity
{
    public class Startup
    {
        #region Properties

        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        #endregion

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddControllersAsServices();
            services.AddJwt();
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.AddRabbitMq();
            containerBuilder.AddMongoRepository<User>("users");
            Container = containerBuilder.Build();
            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //            app.UseHttpsRedirection();
            app.UseRabbitMq();
            app.UseMvc();
            applicationLifetime.ApplicationStopped.Register(Container.Dispose);
        }
    }
}