//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:42 AM
//
// UserServices.cs
//
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.AspNetCore.Identity;

namespace ASMR.Web.Services
{
    public interface IUserService : IServiceBase
    {
        public IQueryable<User> GetAllUsers();

        public Task<User> GetUserById(string id);

        public Task<User> GetUserByName(string userName);

        public Task<IdentityResult> CreateUser(User user, string password);

        public Task<IdentityResult> ModifyUser(string id, User user);
        
        public Task<IdentityResult> ModifyUserPassword(string id, string newPassword);

        public Task<IdentityResult> RemoveUser(string id);

        public Task<IEnumerable<UserRole>> GetUserRoles(User user);
        
        public Task<bool> HasRole(User user, Role role); 

        public Task<IdentityResult> AssignRolesToUser(string id, IEnumerable<Role> roles);

        public Task<User> GetAuthenticatedUser(ClaimsPrincipal principal);
    }

    public class UserService : ServiceBase, IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;

        public UserService(ApplicationDbContext dbContext,
            UserManager<User> userManager,
            RoleManager<UserRole> roleManager) : base(dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IQueryable<User> GetAllUsers()
        {
            return _userManager.Users;
        }

        public Task<User> GetUserById(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public Task<User> GetUserByName(string userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        public Task<IdentityResult> CreateUser(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ModifyUser(string id, User user)
        {
            var entity = await _userManager.FindByIdAsync(id);
            if (entity is null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(user.FirstName))
            {
                entity.FirstName = user.FirstName;
            }

            if (!string.IsNullOrEmpty(user.LastName))
            {
                entity.LastName = user.LastName;
            }
            
            if (!string.IsNullOrEmpty(user.Email))
            {
                entity.Email = user.Email;
            }

            if (!string.IsNullOrEmpty(user.UserName))
            {
                entity.UserName = user.UserName;
            }

            if (!string.IsNullOrEmpty(user.Image))
            {
                entity.Image = user.Image;
            }

            return await _userManager.UpdateAsync(entity);
        }

        public async Task<IdentityResult> ModifyUserPassword(string id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);
        }

        public async Task<IdentityResult> RemoveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            return await _userManager.DeleteAsync(user);
        }

        public async Task<IEnumerable<UserRole>> GetUserRoles(User user)
        {
            var userRoles = new Collection<UserRole>();
            if (user is null)
            {
                return userRoles;
            }
            
            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                var userRole = await _roleManager.FindByNameAsync(role);
                userRoles.Add(userRole);
            }
            
            return userRoles;
        }

        public async Task<bool> HasRole(User user, Role role)
        {
            if (user is null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(user, role.ToString());
        }

        public async Task<IdentityResult> AssignRolesToUser(string id, IEnumerable<Role> roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded || removeResult.Errors.Any())
            {
                return removeResult;
            }

            return await _userManager.AddToRolesAsync(user, roles.Select(role => role.ToString()));
        }

        public Task<User> GetAuthenticatedUser(ClaimsPrincipal principal)
        {
            return principal is null ? null : _userManager.GetUserAsync(principal);
        }
    }
}
