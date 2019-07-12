using Moq;
using StoreData;
using StoreData.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StoreTestProject
{
    public class UserServiceMethodsUnitTests
    {
        [Fact]
        public void GetFirstName_NotNull_FirstName()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(m => m.GetFirstName(1)).Returns("fakeFirstName");

            Assert.NotNull(mock.Object.GetFirstName(1));
            Assert.Equal("fakeFirstName", mock.Object.GetFirstName(1));
        }

        [Fact]
        public void GetLastName_NotNull_LastName()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(m => m.GetLastName(1)).Returns("fakeLastName");

            Assert.NotNull(mock.Object.GetLastName(1));
            Assert.Equal("fakeLastName", mock.Object.GetLastName(1));
        }

        [Fact]
        public void GetUserId_NotNull_UserId()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(m => m.GetUserId("fakeUserName")).Returns(2112);

            Assert.Equal(2112, mock.Object.GetUserId("fakeUserName"));
        }

        [Fact]
        public void GetRoleId_NotNull_RoleId()
        {
            var mock = new Mock<IUserService>();
            mock.Setup(m => m.GetRoleId("fakeRoleName")).Returns(1234);

            Assert.Equal(1234, mock.Object.GetRoleId("fakeRoleName"));
        }

        [Fact]
        public void GetAllUsers_NotNull_UserId()
        {
            Users user1 = new Users { Id = 1, Adress = "fakeAdress" };
            Users user2 = new Users { Id = 2, Adress = "fakeAdress2" };

            List<Users> usersToReturn = new List<Users> { user1, user2 };

            var mock = new Mock<IUserService>();
            mock.Setup(m => m.GetAllUsers()).Returns(usersToReturn);

            Assert.NotEmpty(mock.Object.GetAllUsers());
            Assert.Equal(user1.Id, mock.Object.GetAllUsers().First().Id);
            Assert.Equal(user2.Id, mock.Object.GetAllUsers().Last().Id);
            Assert.Equal(user1.Adress, mock.Object.GetAllUsers().First().Adress);
            Assert.Equal(user2.Adress, mock.Object.GetAllUsers().Last().Adress);
        }
       
        [Fact]
        public void GetUserClaims_NotNull_UserId_ClaimId()
        {
            UserClaims userClaim1 = new UserClaims { Id = 1,UserId = 1};
            UserClaims userClaim2 = new UserClaims { Id = 2, UserId = 2 };

            IList<UserClaims> userClaimsToReturn = new List<UserClaims> { userClaim1, userClaim2 };

            var mock = new Mock<IUserService>();
            mock.Setup(m => m.GetUserClaims(1)).Returns(Task.FromResult(userClaimsToReturn));

            Assert.NotNull(mock.Object.GetUserClaims(1));
            Assert.Equal(userClaim1.Id, mock.Object.GetUserClaims(1).Result.First().Id);
            Assert.Equal(userClaim2.Id, mock.Object.GetUserClaims(1).Result.Last().Id);
            Assert.Equal(userClaim1.UserId, mock.Object.GetUserClaims(1).Result.First().UserId);
            Assert.Equal(userClaim2.Id, mock.Object.GetUserClaims(1).Result.Last().UserId);
        }

        [Fact]
        public void GetUserById_NotNull_UserId()
        {
            Users user = new Users { Id = 1, Adress = "fakeAdress" };

            var mock = new Mock<IUserService>();
            mock.Setup(m => m.GetUserById(1)).Returns(Task.FromResult(user));

            Assert.NotNull(mock.Object.GetUserById(1));
            Assert.Equal(user.Id, mock.Object.GetUserById(1).Result.Id);
        }
        
         [Fact]
        public void GetUserNames_NotNull_UserId()
        {
            Users user1 = new Users { Id = 1, UserName = "fakeUserName" };
            Users user2 = new Users { Id = 2, UserName = "fakeUserName2" };

            List<Users> usersToReturn = new List<Users> { user1, user2 };

            var mock = new Mock<IUserService>();
            mock.Setup(m => m.GetUserNames("userSearch")).Returns(usersToReturn);

            Assert.NotEmpty(mock.Object.GetUserNames("userSearch"));
            Assert.Equal(user1.Id, mock.Object.GetUserNames("userSearch").First().Id);
            Assert.Equal(user2.Id, mock.Object.GetUserNames("userSearch").Last().Id);
            Assert.Equal(user1.UserName, mock.Object.GetUserNames("userSearch").First().UserName);
            Assert.Equal(user2.UserName, mock.Object.GetUserNames("userSearch").Last().UserName);
        }
    }
}
