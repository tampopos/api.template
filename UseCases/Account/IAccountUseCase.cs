using System.Threading.Tasks;

namespace UseCases
{
    public interface IAccountUseCase
    {
        Task<string> SignUpAsync(SignUpArgs args);
        Task<string> SignInAsync(SignInRequest args);
        Task<string> RefreshTokenAsync();
    }
}