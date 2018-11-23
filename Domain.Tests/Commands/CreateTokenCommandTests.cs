using System;
using System.Threading.Tasks;
using Domain.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tmpps.Boardless.Infrastructure.Authentication.Models;
using Tmpps.Infrastructure.JsonWebToken.Interfaces;

namespace Domain.Tests.Commands
{
    [TestClass]
    public class CreateTokenCommandTests
    {
        [TestMethod]
        public async Task ExecuteAsync()
        {
            var userClaim = new UserClaim();
            var expected = Guid.NewGuid().ToString();
            var mock = new Mock<IJwtFactory>();
            mock.Setup(x => x.Create(userClaim)).Returns(expected);
            var command = new CreateTokenCommand(mock.Object);
            var actual = await command.ExecuteAsync(userClaim);
            actual.Is(expected);
        }
    }
}