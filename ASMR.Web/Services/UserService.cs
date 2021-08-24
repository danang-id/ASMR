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

        public Task<User> CreateUser(User user, string password);

        public Task<User> ModifyUser(string id, User user);
        
        public Task<User> ModifyUserPassword(string id, string newPassword);

        public Task<User> RemoveUser(string id);

        public Task<IEnumerable<UserRole>> GetUserRoles(User user);
        
        public Task<bool> HasRole(User user, Role role); 

        public Task<User> AssignRolesToUser(string id, IEnumerable<Role> roles);

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

        public async Task<User> CreateUser(User user, string password)
        {
            await _userManager.CreateAsync(user, password);

            return user;
        }

        public async Task<User> ModifyUser(string id, User user)
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

            await _userManager.UpdateAsync(entity);

            return entity;
        }

        public async Task<User> ModifyUserPassword(string id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);

            return user;
        }

        public async Task<User> RemoveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            await _userManager.DeleteAsync(user);

            return user;
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

        public async Task<User> AssignRolesToUser(string id, IEnumerable<Role> roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            await _userManager.RemoveFromRolesAsync(user, new[]
            {
                Role.Administrator.ToString(),
                Role.Server.ToString(),
                Role.Roaster.ToString()
            });

            await _userManager.AddToRolesAsync(user, roles.Select(role => role.ToString()));

            return user;
        }

        public Task<User> GetAuthenticatedUser(ClaimsPrincipal principal)
        {
            return principal is null ? null : _userManager.GetUserAsync(principal);
        }
    }
}
