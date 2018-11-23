using System.Threading.Tasks;
using Tmpps.Boardless.Infrastructure.Authentication.Models;

namespace Domain.Interfaces.Command
{
    public interface ICreateTokenCommand
    {
        Task<string> ExecuteAsync(UserClaim userClaim);
    }
}