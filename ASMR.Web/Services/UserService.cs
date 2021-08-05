//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/8/2021 7:42 AM
//
// UserServices.cs
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Core.Enumerations;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.EntityFrameworkCore;

namespace ASMR.Web.Services
{
    public interface IUserService : IServiceBase
    {
        public IQueryable<User> GetAllUsers();

        public Task<User> GetUserById(string id);

        public Task<User> GetUserByUsername(string username);

        public Task<User> CreateUser(User user);

        public Task<User> ModifyUser(string id, User user);

        public Task<User> RemoveUser(string id);

        public Task<User> AssignRolesToUser(string id, IEnumerable<Role> roles);

        public Task<User> GetAuthenticatedUser(IIdentity identity);

        public Task<User> ValidateSignIn(string username, string password);
    }

    public class UserService : ServiceBase, IUserService
    {
        private readonly IHashingService _hashingService;

        public UserService(ApplicationDbContext dbContext, IHashingService hashingService) : base(dbContext)
        {
            _hashingService = hashingService;
        }

        public IQueryable<User> GetAllUsers()
        {
            return DbContext.Users
                .Include(entity => entity.Roles)
                .AsQueryable();
        }

        public Task<User> GetUserById(string id)
        {
            return DbContext.Users
                .Include(e => e.Roles)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<User> GetUserByUsername(string username)
        {
            return DbContext.Users
                .Include(e => e.Roles)
                .Where(e => e.Username == username)
                .FirstOrDefaultAsync();
        }

        public async Task<User> CreateUser(User user)
        {
            await DbContext.Users.AddAsync(user);

            return user;
        }

        public async Task<User> ModifyUser(string id, User user)
        {
            var entity = await DbContext.Users
                .Include(e => e.Roles)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
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

            if (!string.IsNullOrEmpty(user.Username))
            {
                entity.Username = user.Username;
            }

            if (!string.IsNullOrEmpty(user.Image))
            {
                entity.Image = user.Image;
            }

            if (!string.IsNullOrEmpty(user.HashedPassword))
            {
                entity.HashedPassword = user.HashedPassword;
            }

            DbContext.Users.Update(entity);

            return entity;
        }

        public async Task<User> RemoveUser(string id)
        {
            var entity = await DbContext.Users
                   .Include(e => e.Roles)
                   .Where(e => e.Id == id)
                   .FirstOrDefaultAsync();
            if (entity is null)
            {
                return null;
            }

            DbContext.Users.Remove(entity);

            return entity;
        }

        public async Task<User> AssignRolesToUser(string id, IEnumerable<Role> roles)
        {
            var user = await DbContext.Users
                .Include(e => e.Roles)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
            if (user is null)
            {
                return null;
            }

            DbContext.UserRoles.RemoveRange(user.Roles);

            var userRoles = new Collection<UserRole>();
            foreach (var role in roles)
            {
                userRoles.Add(new UserRole
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Role = role
                });
            }
            await DbContext.UserRoles.AddRangeAsync(userRoles);

            return user;
        }

        public Task<User> GetAuthenticatedUser(IIdentity identity)
        {
            if (identity is null || !identity.IsAuthenticated)
            {
                return null;
            }

            return DbContext.Users.Include(entity => entity.Roles)
                .Where(entity => entity.Id == identity.Name)
                .FirstOrDefaultAsync();
        }

        public async Task<User> ValidateSignIn(string username, string password)
        {
            var user = await DbContext.Users.Include(entity => entity.Roles)
                .Where(entity => entity.Username == username)
                .FirstOrDefaultAsync();
            if (user is null)
            {
                return null;
            }

            var passwordVerified = _hashingService.VerifyBase64(password, user.HashedPassword);
            if (!passwordVerified)
            {
                return null;
            }

            return user;
        }
    }
}
