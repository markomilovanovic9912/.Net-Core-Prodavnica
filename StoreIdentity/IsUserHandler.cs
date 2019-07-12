/*using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StoreIdentity
{
    public class IsUserHandler : AuthorizationHandler<IsUserRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   IsUserRequirement requirement)
        {
           var user = context.User;

           

           /* if (!context.User.IsInRole(requirement.IsUser).ToString();
            {
                //TODO: Use the following if targeting a version of
                //.NET Framework older than 4.6:
                //      return Task.FromResult(0);
                return Task.CompletedTask;
            }*/

             

            

           /* if (user == "User")
            {
                context.Succeed(requirement);
            }

            //TODO: Use the following if targeting a version of
            //.NET Framework older than 4.6:
            //      return Task.FromResult(0);
            return Task.CompletedTask;
        }


    }
}*/
