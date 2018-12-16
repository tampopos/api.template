using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Domain.Interfaces.Entities;
using Domain.Models;
using Infrastructure.Data.Entities;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Tests.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tmpps.Boardless.Infrastructure.Authentication.Constants;
using Tmpps.Boardless.Infrastructure.Authentication.Models;
using Tmpps.Infrastructure.Common.Cryptography.Interfaces;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;
using Tmpps.Infrastructure.JsonWebToken.Interfaces;

namespace Infrastructure.Data.Tests.Repositories
{
    [TestClass]
    public class UserRepositoryTest
    {
        [DataTestMethod]
        [DynamicData(nameof(CreateAsyncTestParameter))]
        public async Task CreateAsyncTest(UserCreationInfo createUserArgs, Guid userId, User expected)
        {
            using(var context = TestSettings.CreateDbContext())
            {
                var dbContextLazy = context.ConvertLazy();
                var dbContext = dbContextLazy.Value;
                using(var transaction = await dbContext.BeginTransactionAsync())
                {
                    var guidFactoryMock = new Mock<IGuidFactory>();
                    guidFactoryMock.Setup(x => x.CreateNew()).Returns(userId);
                    var mapper = TestSettings.CreateMapper();
                    var hashComputerMock = new Mock<IHashComputer>();
                    hashComputerMock.Setup(x => x.Compute(createUserArgs.Password)).Returns($"Hashed_{createUserArgs.Password}");
                    var claimContextMock = new Mock<IClaimContext<UserClaim>>();
                    var repository = new UserRepository(
                        dbContextLazy,
                        guidFactoryMock.Object,
                        mapper,
                        hashComputerMock.Object,
                        claimContextMock.Object
                    );
                    var actual1 = await repository.CreateAsync(createUserArgs);
                    var actual2 = await repository.GetByIdAsync(userId.ToString());
                    AssertUser(actual1, expected);
                    AssertUser(actual2, expected);
                    transaction.Rollback();
                }
            }
            void AssertUser(IUser e, IUser a)
            {
                if (e == null && a == null)
                {
                    return;
                }
                Assert.AreEqual(e.CultureInfo, a.CultureInfo);
                Assert.AreEqual(e.Email, a.Email);
                Assert.AreEqual(e.EncreptedPassword, a.EncreptedPassword);
                Assert.AreEqual(e.Name, a.Name);
                Assert.AreEqual(e.State, a.State);
                Assert.AreEqual(e.UserId, a.UserId);
            }
        }
        public static IEnumerable<object[]> CreateAsyncTestParameter
        {
            get
            {
                var userId = Guid.NewGuid();
                var info = new UserCreationInfo
                {
                    Email = "test@example.com",
                    Name = "test",
                    CultureInfo = CultureInfo.GetCultureInfo("ja"),
                    Password = "test-password"
                };
                var expected = new User
                {
                    Email = "test@example.com",
                    Name = "test",
                    CultureInfo = CultureInfo.GetCultureInfo("ja"),
                    EncreptedPassword = "Hashed_test-password",
                    State = UserState.Active,
                    UserId = userId.ToString()
                };
                yield return new object[] { info, userId, expected };
            }
        }
    }
}