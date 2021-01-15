using Microsoft.AspNetCore.Identity;
using Moq;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyllabusManager.Tests.TestData;
using Xunit;

namespace SyllabusManager.Tests
{
    public class HelperTests
    {
        [Theory]
        [ClassData(typeof(AdminValidationTestData))]
        public async Task AdminValidationTest(SyllabusManagerUser user, SyllabusManagerUser inputUser, List<string> roles, bool expectedResult)
        {
            // Arrange
            var store = new Mock<IUserStore<SyllabusManagerUser>>();
            var userManagerMock = new Mock<UserManager<SyllabusManagerUser>>(store.Object, null, null, null, null, null, null, null, null);

            userManagerMock
                .Setup(r => r.GetRolesAsync(inputUser ?? user))
                .ReturnsAsync(roles);

            // Act
            var actualResult = await AuthorizationHelper.CheckIfAdmin(user, userManagerMock.Object);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
