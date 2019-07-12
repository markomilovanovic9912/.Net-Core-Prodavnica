 
namespace StoreIdentity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Extensions.Internal;
    using StoreData;
    using StoreData.Models;

    public class UserStore : 
        IUserStore<Users>, 
        IUserPasswordStore<Users>, 
        IUserEmailStore<Users>, 
        IUserLockoutStore<Users>, 
        IUserPhoneNumberStore<Users>,
        IUserLoginStore <Users>,
        IUserClaimStore<Users>
        /*IUserClaimsPrincipalFactory<Users>*/
    {
        private readonly StoreContext db;

        public UserStore(StoreContext db)
        {
            this.db = db;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db?.Dispose();
            }
        }


        #region User
        public Task<string> GetUserIdAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(Users user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(nameof(SetUserNameAsync));
        }



        public Task<string> GetNormalizedUserNameAsync(Users user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException(nameof(GetNormalizedUserNameAsync));
        }

        public Task SetNormalizedUserNameAsync(Users user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)null);
        }

        public async Task<IdentityResult> CreateAsync(Users user, CancellationToken cancellationToken)
        {
            db.Add(user);

            await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> UpdateAsync(Users user, CancellationToken cancellationToken)
        {
            db.Update(user);

            await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(Users user, CancellationToken cancellationToken)
        {
            db.Remove(user);

            int i = await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(i == 1 ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<Users> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (int.TryParse(userId, out int id))
            {
                return await db.Users.FindAsync(id);
            }
            else
            {
                return await Task.FromResult((Users)null);
            }
        }



        public Task SetFirstName(Users user , string FirstName , CancellationToken cancellationToken)
         {

             user.FirstName = FirstName;
             return Task.FromResult((object)null);

         }
        
        public async Task<Users> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await db.Users
                           .AsAsyncEnumerable()
                           .FirstOrDefault(p => p.UserName.Equals(normalizedUserName, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }
        #endregion

        #region Pass
        public Task SetPasswordHashAsync(Users user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult((object)null);
        }

        public Task<string> GetPasswordHashAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }
        #endregion
      
        #region Mail
        public Task SetEmailAsync(Users user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(Users user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<Users> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(Users user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(Users user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)null);
        }
        #endregion
       
        #region Lockout
        public Task<DateTimeOffset?> GetLockoutEndDateAsync(Users user, CancellationToken cancellationToken)
        {

            return Task.FromResult(user.LockoutEnd);   

        }

        public Task SetLockoutEndDateAsync(Users user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;

            return Task.FromResult((object)null);
        }

        public Task<int> IncrementAccessFailedCountAsync(Users user, CancellationToken cancellationToken)
        {
            user.LoginTry++;
            return Task.FromResult(user.LoginTry);
        }

        public Task ResetAccessFailedCountAsync(Users user, CancellationToken cancellationToken)
        {
            user.LoginTry = 0;
            return Task.FromResult<object>(null);
        }

        public Task<int> GetAccessFailedCountAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LoginTry);
        }

        public Task<bool> GetLockoutEnabledAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(Users user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = true;

            return Task.FromResult((object)null);
        }
        #endregion
       
        #region Phone
        public Task SetPhoneNumberAsync(Users user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;

            return Task.FromResult((object)null);
        }

        public Task<string> GetPhoneNumberAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(Users user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(Users user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;

            return Task.FromResult((object)null);
        }
        #endregion

        #region External login
       public Task AddLoginAsync(Users user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            var l = new ExternalUserLogins
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName
            };
           
            db.ExternalUserLogins.Add(l);
             
            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(Users user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(Users user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Users> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var userLogin = db.ExternalUserLogins.FirstOrDefault(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);

            if (userLogin != null)
            {
                return await GetUserAggregate(u => u.Id.Equals(userLogin.UserId), cancellationToken);
            }

            return null;

        }

        protected virtual Task<Users> GetUserAggregate(Expression<Func<Users, bool>> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(Userss.FirstOrDefault(filter));

        }

        public IQueryable<Users> Userss
        {
            get { return db.Set<Users>(); }
        }
        #endregion

        #region Claim
        public Task<Users> AddClaimAsync(Users user, Claim claim)
        {

            /*var claimToAdd = db.UserClaims.Where(uc => uc.UserId == user.Id).Select(c => new Claim (c.ClaimType,c.ClaimValue)).ToList();*/

            /*
            var claimToAdd = new UserClaims
            {
                UserId = user.Id,
                ClaimType = claim.
                ClaimValue = "U21341243r"
            };
            */
           /* db.UserClaims.Add(claimToAdd);
            db.SaveChanges();*/

            return null;

        }   

        public Task<IList<Claim>> GetClaimsAsync(Users user, CancellationToken cancellationToken)
        {
            var result = db.UserClaims.Where(uc => uc.UserId == user.Id).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            return Task.FromResult((IList<Claim>)result);

        }

        public Task AddClaimsAsync(Users user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (var claim in claims)
            {
                db.UserClaims.Add(new UserClaims { UserId = user.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
            }
            return Task.FromResult(0);
        }

        public Task ReplaceClaimAsync(Users user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(Users user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (var claim in claims)
            {
                db.UserClaims.Remove(new UserClaims { UserId = user.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
            }
            return Task.FromResult(0);
        }

        public Task<IList<Users>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        
        #endregion



        /*#region Claims
         public Task<IList<Claim>> GetClaimsAsync(Users user, CancellationToken cancellationToken)
         {

             ThrowIfDisposed();
             if (user == null)
             {
                 throw new ArgumentNullException(nameof(user));
             }
             if (claims == null)
             {
                 throw new ArgumentNullException(nameof(claims));
             }
             foreach (var claim in claims)
             {
                 UserClaims.Add(CreateUserClaim(user, claim));
             }
             return Task.FromResult(false);
         }

         public Task AddClaimsAsync(Users user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
         {


             if (user == null)
             {
                 throw new ArgumentNullException(nameof(user));
             }
             if (claims == null)
             {
                 throw new ArgumentNullException(nameof(claims));
             }
             foreach (var claim in claims)
             {
                 user.Add(CreateUserClaim(user, claim));
             }
             return Task.FromResult(false);
         }

         public Task ReplaceClaimAsync(Users user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
         {

               var matchedClaims = UserClaims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToList();
                 foreach (var matchedClaim in matchedClaims)
                 {
                     matchedClaim.ClaimValue = newClaim.Value;
                     matchedClaim.ClaimType = newClaim.Type;
                 }

                 return Task.FromResult(0);

         }

         public Task RemoveClaimsAsync(Users user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
         {
             throw new NotImplementedException();
         }

         public Task<IList<Users>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
         {
             throw new NotImplementedException();
         }

         public Task<ClaimsPrincipal> CreateAsync(Users user)
         {
             throw new NotImplementedException();
         }

     

        #endregion*/
    }
}
