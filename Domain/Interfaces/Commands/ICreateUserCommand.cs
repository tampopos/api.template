using System.Threading.Tasks;
using Domain.Models;
using Tmpps.Boardless.Infrastructure.Authentication.Models;

namespace Domain.Interfaces.Command
{
    public interface ICreateUserCommand
    {
        Task<UserClaim> ExecuteAsync(UserCreationInfo userCreationInfo);
        Task SendMailForNewUserAsync(UserClaim claim);
    }
}