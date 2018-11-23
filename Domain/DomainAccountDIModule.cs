using System.Runtime.CompilerServices;
using Domain.Commands;
using Domain.Interfaces.Command;
using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
[assembly : InternalsVisibleTo("Domain.Tests")]
namespace Domain
{
    public class DomainAccountDIModule : IDIModule
    {
        public void DefineModule(IDIBuilder builder)
        {
            builder.RegisterType<CreateTokenCommand>(x => x.As<ICreateTokenCommand>());
            builder.RegisterType<CreateUserCommand>(x => x.As<ICreateUserCommand>());
        }
    }
}