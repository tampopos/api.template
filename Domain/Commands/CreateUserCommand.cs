using System.Threading.Tasks;
using Domain.Interfaces.Command;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Tmpps.Boardless.Infrastructure.Authentication.Models;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;
using Tmpps.Infrastructure.SQS.Interfaces;

namespace Domain.Commands
{
    internal class CreateUserCommand : ICreateUserCommand
    {
        private IUserRepository userRepository;
        private IMapper mapper;
        private IMessageSender messageSender;

        public CreateUserCommand(
            IUserRepository userRepository,
            IMapper mapper,
            IMessageSender messageSender)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.messageSender = messageSender;
        }

        public async Task<UserClaim> ExecuteAsync(UserCreationInfo userCreationInfo)
        {
            var user = await this.userRepository.CreateAsync(userCreationInfo);
            return this.mapper.Map<UserClaim>(user);
        }

        public async Task SendMailForNewUserAsync(UserClaim claim)
        {
            var mail = this.mapper.Map<WelcomeMailArgs>(claim);
            await this.messageSender.SendAsync(mail);
        }
    }
}