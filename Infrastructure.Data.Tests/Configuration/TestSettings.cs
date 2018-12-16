using System;
using System.IO;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Tmpps.Infrastructure.Common.Foundation;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;
using Tmpps.Infrastructure.Data.Entity.Interfaces;
using Tmpps.Infrastructure.Data.Interfaces;
using Tmpps.Infrastructure.Npgsql.Entity.Wrapper;

namespace Infrastructure.Data.Tests.Configuration
{
    public static class TestSettings
    {
        public static string RootPath => Directory.GetCurrentDirectory();

        private static readonly IConfigurationRoot _configurationRoot = CreateConfigurationRoot();
        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static IConfigurationRoot CreateConfigurationRoot()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(RootPath)
                .AddJsonFile("appsettings.json", optional : true, reloadOnChange : true)
                .AddEnvironmentVariables()
                .AddJsonFile($"appsettings.user.json", optional : true);
            return builder.Build();
        }

        public static DbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BoardlessDataContext>();
            var connectionString = _configurationRoot.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
            return new BoardlessDataContext(optionsBuilder.Options);
        }

        public static IDbContext Convert<TDbContext>(this TDbContext context, CancellationTokenSource tokenSource = null, IDbQueryCache queryPool = null)
        where TDbContext : DbContext
        {
            return new DbContextWrapper<TDbContext>(context, tokenSource ?? _cancellationTokenSource, queryPool ?? CreateDbQueryCacheMock());
        }
        public static Lazy<IDbContext> ConvertLazy<TDbContext>(this TDbContext context, CancellationTokenSource tokenSource = null, IDbQueryCache queryPool = null)
        where TDbContext : DbContext
        {
            return new Lazy<IDbContext>(() => context.Convert(tokenSource, queryPool), true);
        }

        private static IDbQueryCache CreateDbQueryCacheMock()
        {
            var mock = new Mock<IDbQueryCache>();
            return mock.Object;
        }

        public static IMapper CreateMapper()
        {
            return new Mapper();
        }
    }
}