using System;
using System.Threading.Tasks;
using Domain.Interfaces.Command;
using Domain.Models;
using Tmpps.Boardless.Infrastructure.Authentication.Models;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;
using Tmpps.Infrastructure.Data.Entity.Interfaces;
using UseCases.Interfaces.Queries;

namespace UseCases
{
    internal class AccountUseCase : IAccountUseCase
    {
        private Lazy<IDbTransactionManager> dbTransationManagerLazy;
        private IMapper mapper;
        private ICreateUserCommand createUserCommand;
        private ICreateTokenCommand createTokenCommand;
        private IUserQuery userQuery;

        public AccountUseCase(
            Lazy<IDbTransactionManager> dbTransationManagerLazy,
            IMapper mapper,
            ICreateUserCommand createUserCommand,
            ICreateTokenCommand createTokenCommand,
            IUserQuery userQuery)
        {
            this.dbTransationManagerLazy = dbTransationManagerLazy;
            this.mapper = mapper;
            this.createUserCommand = createUserCommand;
            this.createTokenCommand = createTokenCommand;
            this.userQuery = userQuery;
        }

        private IDbTransactionManager DbTransationManager => this.dbTransationManagerLazy.Value;

        public async Task<string> SignUpAsync(SignUpArgs args)
        {
            var creationInfo = this.mapper.Map<UserCreationInfo>(args);
            var claim = await ExecuteAsync();
            await this.createUserCommand.SendMailForNewUserAsync(claim);
            return await this.createTokenCommand.ExecuteAsync(claim);

            async Task<UserClaim> ExecuteAsync()
            {
                var trn = await this.DbTransationManager.BeginTransactionAsync();
                try
                {
                    var res = await this.createUserCommand.ExecuteAsync(creationInfo);
                    trn.Commit();
                    return res;
                }
                catch (Exception)
                {
                    trn.Rollback();
                    throw;
                }
                finally
                {
                    trn.Dispose();
                }
            }
        }

        public async Task<string> SignInAsync(SignInRequest args)
        {
            var userClaim = await this.userQuery.GetSignInUserClaimAsync(args);
            return await this.createTokenCommand.ExecuteAsync(userClaim);
        }

        public async Task<string> RefreshTokenAsync()
        {
            var userClaim = await this.userQuery.GetCurrentUserClaimAsync();
            return await this.createTokenCommand.ExecuteAsync(userClaim);
        }
    }
}