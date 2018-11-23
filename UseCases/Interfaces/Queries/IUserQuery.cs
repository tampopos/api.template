using System.Threading.Tasks;
using Tmpps.Boardless.Infrastructure.Authentication.Models;
using UseCases;

namespace UseCases.Interfaces.Queries
{
    public interface IUserQuery
    {
        Task<UserClaim> GetCurrentUserClaimAsync();
        Task<UserClaim> GetSignInUserClaimAsync(SignInRequest args);
    }
}