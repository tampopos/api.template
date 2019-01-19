using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tmpps.Boardless.Infrastructure.Authentication.Models;
using Tmpps.Infrastructure.AspNetCore.Configuration.Interfaces;
using Tmpps.Infrastructure.AspNetCore.Extensions;
using Tmpps.Infrastructure.AspNetCore.Middlewares.Extensions;
using Tmpps.Infrastructure.Autofac.Builder;
using Tmpps.Infrastructure.Common.Configuration;
using Tmpps.Infrastructure.Common.DependencyInjection.Interfaces;
using Tmpps.Infrastructure.SQS.Interfaces;

namespace Api.Configuration
{
    public class Startup
    {
        private IHostingEnvironment hostingEnvironment;
        private ILoggerFactory loggerFactory;
        private IConfiguration configuration;
        private IConfigurationRoot configurationRoot;
        private Assembly executeAssembly;
        private string rootPath;
        private ApiConfig config;

        public Startup(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
            this.executeAssembly = Assembly.GetEntryAssembly();
            this.rootPath = Directory.GetCurrentDirectory();
            this.configurationRoot = this.CreateConfigurationRoot();
        }

        private IConfigurationRoot CreateConfigurationRoot()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional : true, reloadOnChange : true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional : true)
                .AddEnvironmentVariables()
                .AddJsonFile($"appsettings.user.json", optional : true);
            return builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            this.config = new ApiConfig(this.configurationRoot);
            services.AddCors();
            services.AddJwtAuthentication(this.config);
            services.AddMvc().AddControllersAsServices();

            var builder = new AutofacBuilder();
            builder.Populate(services);
            builder.RegisterModule(new ApiDIModule(this.executeAssembly, this.rootPath, this.configurationRoot, this.loggerFactory));
            var scope = builder.Build();
            this.Subscribe(scope);
            return builder.CreateServiceProvider();
        }

        private void Subscribe(IScope scope)
        {
            var cts = scope.Resolve<CancellationTokenSource>();
            AssemblyLoadContext.Default.Unloading += context => cts.Cancel(false);
            var subscriber = scope.Resolve<IMessageSubscriber>();
            subscriber.SubscribeAsync();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseRequestLogger();
            app.UseCors(this.config as ICorsConfig);
            app.UseJwtAuthentication<UserClaim>();
            app.UseMvc(this.config);
        }
    }
}