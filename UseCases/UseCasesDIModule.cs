using Tmpps.Infrastructure.Common.DependencyInjection.Builder.Interfaces;
using UseCases;

namespace UseCases
{
    public class UseCasesDIModule : IDIModule
    {
        public void DefineModule(IDIBuilder builder)
        {
            builder.RegisterType<AccountUseCase>(x => x.As<IAccountUseCase>());
        }
    }
}