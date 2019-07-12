using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using StoreData;
using StoreData.Models;

namespace StoreIdentity
{

    public class ExternalUserLoginsStore 
    {
        private readonly StoreContext db;

        public ExternalUserLoginsStore(StoreContext db)
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



        public Task AddLoginAsync(ExternalUserLogins user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            var userLogin = new ExternalUserLogins
            {
                UserId = user.Id,
                LoginProvider = user.LoginProvider,
                ProviderDisplayName = user.ProviderDisplayName,
                ProviderKey = user.ProviderKey,
            };
            db.ExternalUserLogins.Add(userLogin);

            return Task.FromResult(0);
        }

        public async Task<IdentityResult> CreateAsync(ExternalUserLogins user, CancellationToken cancellationToken)
        {
            db.Add(user);

            await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(ExternalUserLogins user, CancellationToken cancellationToken)
        {
            db.Remove(user);

            int i = await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(i == 1 ? IdentityResult.Success : IdentityResult.Failed());
        }

        public async Task<IdentityResult> UpdateAsync(ExternalUserLogins user, CancellationToken cancellationToken)
        {
            db.Update(user);

            await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public Task<ExternalUserLogins> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ExternalUserLogins> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ExternalUserLogins> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(ExternalUserLogins user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(ExternalUserLogins user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(ExternalUserLogins user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(ExternalUserLogins user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(ExternalUserLogins user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(ExternalUserLogins user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(ExternalUserLogins user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }

}