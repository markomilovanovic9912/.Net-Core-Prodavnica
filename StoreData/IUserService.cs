using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoreData 
{
    public interface IUserService
    {

        string SetFirstName(int userId, string firstName);
        string SetLastName(int userId, string lastName);
        string GetFirstName(int userId);
        string GetLastName(int userId);
        string SetAdress(int userId, string Adress);
        string SetCity(int userId,string City);
        string SetCountryOrState(int userId,string CountryOrState);
        int GetUserId(string userName);
        int GetRoleId(string roleName);
        Task AddUserToRole(UserRole Role);
        IEnumerable<Users> GetAllUsers();
        IEnumerable<Users> GetUserNames(string userSearch);
        Task<IList<UserClaims>> GetUserClaims(int userId);
        Task AddUserClaim(UserClaims claim);
        Task RemoveUserClaim(UserClaims claim);
        Task RemoveUser(int userId);
        Task<Users> GetUserById(int userId);
        Task<Users> AddUser(Users user);
        Users GetUserByUserName(string userName);



    }
}
