using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StoreData.Models;

namespace StoreData
{
    public interface IExternalUserLoginsStoreService
    {

        Task AddLogin (ExternalUserLogins login , int userId);

    }
}
