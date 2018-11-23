using System.Threading.Tasks;
using Domain.Interfaces.Entities;
using Domain.Models;
using Tmpps.Infrastructure.Data.Interfaces;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<IUser>
    {
        Task<IUser> CreateAsync(UserCreationInfo user);
    }
}