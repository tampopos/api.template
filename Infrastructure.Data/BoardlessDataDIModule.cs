using Domain.Interfaces.Repositories;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
using Tmpps.Infrastructure.Data;
using Tmpps.Infrastructure.Data.Entity.Interfaces;
using Tmpps.Infrastructure.Data.Interfaces;
using Tmpps.Infrastructure.Npgsql.Entity;
using Tmpps.Infrastructure.Npgsql.Entity.Wrapper;
using UseCases.Interfaces.Queries;

namespace Infrastructure.Data
{
    public class BoardlessDataDIModule : IDIModule
    {
        private string connectionString;

        public BoardlessDataDIModule(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void DefineModule(IDIBuilder builder)
        {
            builder.RegisterType<UserRepository>(x => x.As<IUserRepository>().As<IUserQuery>());
            var optionsBuilder = new DbContextOptionsBuilder<BoardlessDataContext>();
            optionsBuilder.UseNpgsql(this.connectionString);
            builder.RegisterInstance(optionsBuilder.Options);
            builder.RegisterType<BoardlessDataContext>(x => x.InstancePerLifetimeScope());
            builder.RegisterType<DbContextWrapper<BoardlessDataContext>>(x => x.As<IDbContext>().As<IDbTransactionManager>().InstancePerLifetimeScope());
            builder.RegisterType<DbQueryCache>(x => x.As<IDbQueryCache>().SingleInstance());
        }
    }
}