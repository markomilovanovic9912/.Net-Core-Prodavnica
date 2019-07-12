using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StoreData;
using StoreData.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Linq.Expressions;

namespace StoreIdentity
{

    public class RoleStore : IRoleStore<UserRole>
    {

        private readonly StoreContext db;

        public RoleStore(StoreContext db)
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


        public async Task<IdentityResult> CreateAsync(UserRole role, CancellationToken cancellationToken)
        {
            db.Add(role);

            await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> UpdateAsync(UserRole role, CancellationToken cancellationToken)
        {
            db.Update(role);

            await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(UserRole role, CancellationToken cancellationToken)
        {
            db.Remove(role);

            await db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public Task<string> GetRoleIdAsync(UserRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(UserRole role, CancellationToken cancellationToken)
        {
            var name = db.Roles.FirstOrDefault(r => r.Id == role.RoleId).Name.ToString();

            return Task.FromResult(name);
        }

        public Task SetRoleNameAsync(UserRole role, string roleName, CancellationToken cancellationToken)
        {
            var name = db.Roles.FirstOrDefault(r => r.Id == role.RoleId);

            name.Name = roleName;
            db.SaveChanges();

            return Task.FromResult(0);
        }

        public Task<string> GetNormalizedRoleNameAsync(UserRole role, CancellationToken cancellationToken)
        {
            var normName = db.Roles.FirstOrDefault(r => r.Id == role.RoleId).NormalizedName.ToString();

            return Task.FromResult(normName);
        }

        public Task SetNormalizedRoleNameAsync(UserRole role, string normalizedName, CancellationToken cancellationToken)
        {
            var name = db.Roles.FirstOrDefault(r => r.Id == role.RoleId);

            name.NormalizedName = normalizedName;
            db.SaveChanges();

            return Task.FromResult(0);
        }

        public async Task<UserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var id = Int32.Parse(roleId);

            var role = db.Roles.FirstOrDefault();

            if (role != null)
            {
                return await GetUserAggregate(r => r.Id.Equals(id) , cancellationToken);
            }

            return null;
        }

        public async Task<UserRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
             

            var role = db.Roles.FirstOrDefault();

            if (role != null)
            {
                return await GetUserAggregate(r => r.Role.NormalizedName.ToUpper().Equals(normalizedRoleName.ToUpper()), cancellationToken);
            }

            return null;
        }

        /* var role = db.UserRoles.Where(r => r.Role.NormalizedName == normalizedRoleName);

            if (role != null)
            {

                return await Task.FromResult(role);
            }

            return null;*/


        protected virtual Task<UserRole> GetUserAggregate(Expression<Func<UserRole, bool>> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(Userss.FirstOrDefault(filter));

        }

        public IQueryable<UserRole> Userss
        {
            get { return db.Set<UserRole>(); }
        }
    }
}
