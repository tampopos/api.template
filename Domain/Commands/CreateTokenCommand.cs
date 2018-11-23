using System.Threading.Tasks;
using Domain.Interfaces.Command;
using Tmpps.Boardless.Infrastructure.Authentication.Models;
using Tmpps.Infrastructure.JsonWebToken.Interfaces;

namespace Domain.Commands
{
    internal class CreateTokenCommand : ICreateTokenCommand
    {
        private IJwtFactory jwtFactory;

        public CreateTokenCommand(IJwtFactory jwtFactory)
        {
            this.jwtFactory = jwtFactory;
        }

        public async Task<string> ExecuteAsync(UserClaim userClaim)
        {
            return await Task.FromResult(this.jwtFactory.Create(userClaim));
        }
    }
}