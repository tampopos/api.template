using System.Reflection;
using System.Threading;
using Api.Messaging;
using Domain;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tmpps.Boardless.Infrastructure.Authentication;
using Tmpps.Infrastructure.AspNetCore.Configuration.Interfaces;
using Tmpps.Infrastructure.Autofac;
using Tmpps.Infrastructure.Autofac.Configuration;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
using Tmpps.Infrastructure.Common.Foundation;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;
using Tmpps.Infrastructure.Data.Configuration.Interfaces;
using Tmpps.Infrastructure.Data.Migration.Interfaces;
using Tmpps.Infrastructure.JsonWebToken;
using Tmpps.Infrastructure.JsonWebToken.Interfaces;
using Tmpps.Infrastructure.Npgsql.Entity.Migration;
using Tmpps.Infrastructure.SQS;
using Tmpps.Infrastructure.SQS.Interfaces;
using UseCases;

namespace Api.Configuration
{
    public class ApiDIModule : IDIModule
    {
        private Assembly executeAssembly;
        private string rootPath;
        private CommonDIModule commonAutofacModule;
        private IConfigurationRoot configurationRoot;

        public ApiDIModule(
            Assembly executeAssembly,
            string rootPath,
            IConfigurationRoot configurationRoot,
            ILoggerFactory loggerFactory)
        {
            this.executeAssembly = executeAssembly;
            this.rootPath = rootPath;
            this.commonAutofacModule = new CommonDIModule(executeAssembly, rootPath, loggerFactory);
            this.configurationRoot = configurationRoot;
        }

        public void DefineModule(IDIBuilder builder)
        {
            var mapRegister = new MapRegister();
            builder.RegisterInstance(mapRegister, x => x.As<IMapRegister>());
            builder.RegisterModule(this.commonAutofacModule);
            builder.RegisterModule(new AutofacDIModule());
            builder.RegisterModule(new JwtDIModule());
            builder.RegisterModule(new BoardlessDataDIModule(this.configurationRoot.GetConnectionString("DefaultConnection")));
            builder.RegisterModule(new SQSDIModule());
            builder.RegisterModule(new BoardlessAuthenticationDIModule(LifetimeType.Scoped));
            builder.RegisterModule(new DomainAccountDIModule());
            builder.RegisterModule(new UseCasesDIModule());
            builder.RegisterInstance(this.configurationRoot, x => x.As<IConfigurationRoot>());
            builder.RegisterType<ApiConfig>(x =>
                x.As<IDbConfig>()
                .As<IWebConfig>()
                .As<IJwtConfig>()
                .As<ISQSConfig>()
                .SingleInstance());
            builder.RegisterType<WelcomeMailSender>(x => x.As<IMessageReceiver<WelcomeMailArgs>>());
            builder.RegisterType<CancellationTokenSource>(x => x.SingleInstance());
        }
    }
}