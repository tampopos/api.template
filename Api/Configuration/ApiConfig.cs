using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Tmpps.Infrastructure.AspNetCore.Configuration.Interfaces;
using Tmpps.Infrastructure.Common.Configuration.Interfaces;
using Tmpps.Infrastructure.Data.Configuration.Interfaces;
using Tmpps.Infrastructure.JsonWebToken.Interfaces;
using Tmpps.Infrastructure.SQS;
using Tmpps.Infrastructure.SQS.Interfaces;
using Tmpps.Infrastructure.SQS.Models;

namespace Api.Configuration
{
    public class ApiConfig : IWebConfig, IConfig, IDbConfig, IJwtConfig, ISQSConfig
    {
        protected IConfigurationRoot configurationRoot;
        public ApiConfig(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
            this.SqlPoolPath = this.configurationRoot.GetValue<string>(nameof(this.SqlPoolPath));
            this.JwtSecret = this.configurationRoot.GetValue<string>(nameof(this.JwtSecret));
            this.JwtExpiresDate = this.configurationRoot.GetValue<int>(nameof(this.JwtExpiresDate));
            this.JwtAudience = this.configurationRoot.GetValue<string>(nameof(this.JwtAudience));
            this.JwtIssuer = this.configurationRoot.GetValue<string>(nameof(this.JwtIssuer));
            this.SystemDateTime = this.configurationRoot.GetValue<DateTime?>(nameof(this.SystemDateTime));
            this.AwsAccessKeyId = this.configurationRoot.GetValue<string>(nameof(this.AwsAccessKeyId));
            this.AwsSecretAccessKey = this.configurationRoot.GetValue<string>(nameof(this.AwsSecretAccessKey));
            this.ServiceURL = this.configurationRoot.GetValue<string>(nameof(this.ServiceURL));
            this.SQSMessageSendSettings = this.configurationRoot.GetSQSMessageSendSettings(nameof(this.SQSMessageSendSettings));
            this.SQSMessageReceiveSettings = this.configurationRoot.GetSQSMessageReceiveSettings(nameof(this.SQSMessageReceiveSettings));
            this.MaxConcurrencyReceive = this.configurationRoot.GetValue<int>(nameof(this.MaxConcurrencyReceive));
            this.IsEnableCors = this.configurationRoot.GetValue<bool>(nameof(IsEnableCors));
            this.UseAuthentication = this.configurationRoot.GetValue<bool>(nameof(UseAuthentication));
            this.IsUseSecure = this.configurationRoot.GetValue<bool>(nameof(IsUseSecure));
            this.CorsOrigins = this.configurationRoot.GetValue<string>(nameof(CorsOrigins));
        }

        public string SqlPoolPath { get; }
        public string JwtSecret { get; }
        public int JwtExpiresDate { get; }
        public string JwtAudience { get; }
        public string JwtIssuer { get; }
        public DateTime? SystemDateTime { get; }
        public string AwsAccessKeyId { get; }
        public string AwsSecretAccessKey { get; }
        public string ServiceURL { get; }
        public IDictionary<string, SQSMessageSendSetting> SQSMessageSendSettings { get; }
        public IDictionary<string, SQSMessageReceiveSetting> SQSMessageReceiveSettings { get; }
        public int MaxConcurrencyReceive { get; }

        public string GetConnectionString(string name)
        {
            return this.configurationRoot.GetConnectionString(name);
        }
        public bool IsEnableCors { get; set; }
        public bool UseAuthentication { get; set; }
        public bool IsUseSecure { get; set; }
        public string CorsOrigins { get; set; }

        public IEnumerable<string> GetCorsOrigins()
        {
            return this.CorsOrigins?.Split(",") ?? Enumerable.Empty<string>();
        }

        public void CreateMvcConfigureRoutes(IRouteBuilder routes)
        {
            routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}