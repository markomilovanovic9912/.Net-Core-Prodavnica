using StoreData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using StoreData.Models;
using Microsoft.EntityFrameworkCore;

namespace StoreServices
{
    public class UserService : IUserService
    {

        private StoreContext _context;

        public UserService(StoreContext context)
        {
            _context = context;
        }

        public string SetFirstName(int userId, string firstName)
        {
            var db = _context.Users.First(User => User.Id == userId);

            db.FirstName = firstName;
            _context.SaveChanges();

            return "Success";
        }

        public string SetLastName(int userId, string lastName)
        {
          var db = _context.Users.First(User => User.Id == userId);

            db.LastName = lastName;
            _context.SaveChanges();

            return "Success";
        }

        public string GetFirstName(int userId)
        {
            var db = _context.Users.First(User => User.Id == userId);

            return db.FirstName;

        }


        public string GetLastName(int userId)
        {
            var db = _context.Users.First(User => User.Id == userId);

            return db.LastName;

        }

        public Task AddUserToRole(UserRole Role)
        {
            var role = new UserRole
            {
                RoleId = Role.RoleId,
                UserId = Role.UserId
            };
            _context.Add(role);
            _context.SaveChanges();
            return Task.FromResult("Sucess");


        }

        public int GetUserId(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            var userId = user.Id;

            return userId;
        }

        public int GetRoleId(string roleName)
        {
            var role = _context.Roles.FirstOrDefault(u => u.Name == roleName);

            var roleId = role.Id;

            return roleId;
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return _context.Users.Include(u => u.UserClaims).Include(u => u.UserRoles);
        }    


        public Task AddUserClaim(UserClaims claim)
        {
            var claimToAdd = new UserClaims
            {
                ClaimType = claim.ClaimType,
                ClaimValue = claim.ClaimValue,
                UserId = claim.UserId
            };

            _context.UserClaims.Add(claimToAdd);
            _context.SaveChanges();

            return Task.FromResult("Sucess");

        }

        public Task RemoveUserClaim(UserClaims claim)
        {
            var claimToRemove = _context.UserClaims.FirstOrDefault(uc => uc.Id == claim.Id);


            _context.UserClaims.Remove(claimToRemove);
            _context.SaveChanges();

            return Task.FromResult("Sucess");
            
        }

        public Task<IList<UserClaims>> GetUserClaims(int userId)
        {

            var result = _context.UserClaims.Where(uc => uc.UserId == userId).Select(c => new UserClaims
            { ClaimType = c.ClaimType,
               ClaimValue = c.ClaimValue,
               UserId = c.UserId,
               Id = c.Id
            }).ToList();
            return Task.FromResult((IList<UserClaims>)result);
        }

        public Task RemoveUser(int userId)
        {
            var userToRemove = _context.Users.FirstOrDefault(u => u.Id == userId);

            _context.Remove(userToRemove);
            _context.SaveChanges();

            return Task.FromResult("Sucess");

        }

        public Task<Users> GetUserById(int userId)
        {
            var getUser = _context.Users.FirstOrDefault(u => u.Id == userId);

            return Task.FromResult(getUser);
        }

        public string SetAdress(int userId, string Adress)
        {
            var location = _context.Users.FirstOrDefault(u => u.Id == userId);

            location.Adress = Adress;
            _context.SaveChanges();

            return "Sucess";
        }
 
        public string SetCity(int userId, string City)
        {
            var location = _context.Users.FirstOrDefault(u => u.Id == userId);

            location.City = City;
            _context.SaveChanges();

            return "Sucess";
        }

        public string SetCountryOrState(int userId, string CountryOrState)
        {
            var location = _context.Users.FirstOrDefault(u => u.Id == userId);

            location.CountryOrState = CountryOrState;
            _context.SaveChanges();

            return "Sucess";
        }

        public IEnumerable<Users> GetUserNames(string userSearch)
        {
            return _context.Users.Include(u => u.UserClaims).Include(u => u.UserRoles).Where(u => u.UserName.Contains(userSearch));
        }

        //Method For Testing Purposes Only!! 
        public Task<Users> AddUser(Users user)
        {
            var userToAdd = new Users
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = true,
                LockoutEnabled = true,
                LoginTry = 1,
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEEnrO2rri52xI85tO/qsuFFfPaQ4P0w3rP2uq8JyUwuZugNGjbFcWJqvp7qGoaySfw=="

            };

            _context.Add(userToAdd);
            _context.SaveChanges();

            return null;
        }

        public Users GetUserByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);

        }
    }
}
