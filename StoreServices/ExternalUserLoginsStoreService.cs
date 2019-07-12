using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StoreServices
{
    public class ExternalUserLoginsStoreService: IExternalUserLoginsStoreService
    {

        private StoreContext _context;

        public ExternalUserLoginsStoreService(StoreContext context)
        {
            _context = context;
        }

        public Task AddLogin(ExternalUserLogins login, int userId)
        {
            var l = new ExternalUserLogins
            {
                UserId = userId,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName
            };
             
            _context.ExternalUserLogins.Add(l);

            return Task.FromResult(0);

        }

    }
}
