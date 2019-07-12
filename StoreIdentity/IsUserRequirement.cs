using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreIdentity
{
    public class IsUserRequirement : IAuthorizationRequirement
    {
        public string IsUser { get; private set; }

        public IsUserRequirement(string isUser)
        {
            IsUser = isUser;
        }

    }
}
