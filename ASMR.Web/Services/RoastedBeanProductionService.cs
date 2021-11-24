//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 5/23/2021 11:54 PM
//
// ProductionService.cs
//

using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.EntityFrameworkCore;

namespace ASMR.Web.Services
{
    public interface IRoastedBeanProductionService : IServiceBase
    {
        public IQueryable<RoastedBeanProduction> GetAllRoastedBeanProductions();
        
        public IQueryable<RoastedBeanProduction> GetRoastedBeanProductionByUser(string userId);
        
        public Task<RoastedBeanProduction> GetRoastedBeanProductionById(string id);

        public Task<RoastedBeanProduction> CreateRoastedBeanProduction(RoastedBeanProduction roastedBeanProduction);

        public Task<RoastedBeanProduction> ModifyRoastedBeanProduction(string id,
            RoastedBeanProduction roastedBeanProduction);
    }

    public class RoastedBeanProductionService : ServiceBase, IRoastedBeanProductionService
    {
        public RoastedBeanProductionService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<RoastedBeanProduction> GetAllRoastedBeanProductions()
        {
            return DbContext.RoastedBeanProductions
                .Include(e => e.User)
                .Include(e => e.Bean)
                    .ThenInclude(e => e.Inventory)
                .AsQueryable();
        }

        public IQueryable<RoastedBeanProduction> GetRoastedBeanProductionByUser(string userId)
        {
            return DbContext.RoastedBeanProductions
                .Where(e => e.UserId == userId)
                .Include(e => e.User)
                .Include(e => e.Bean)
                    .ThenInclude(e => e.Inventory)
                .AsQueryable();
        }

        public Task<RoastedBeanProduction> GetRoastedBeanProductionById(string id)
        {
            return DbContext.RoastedBeanProductions
                .Where(e => e.Id == id)
                .Include(e => e.User)
                .Include(e => e.Bean)
                    .ThenInclude(e => e.Inventory)
                .FirstOrDefaultAsync();
        }

        public async Task<RoastedBeanProduction> CreateRoastedBeanProduction(RoastedBeanProduction roastedBeanProduction)
        {
            var entityEntry = await DbContext.RoastedBeanProductions.AddAsync(roastedBeanProduction);
            return entityEntry.Entity;
        }

        public async Task<RoastedBeanProduction> ModifyRoastedBeanProduction(string id,
            RoastedBeanProduction roastedBeanProduction)
        {
            var entity = await DbContext.RoastedBeanProductions
                .Where(e => e.Id == id)
                .Include(e => e.User)
                .Include(e => e.Bean)
                    .ThenInclude(e => e.Inventory)
                .FirstOrDefaultAsync();
            if (entity is null)
            {
                return null;
            }

            if (roastedBeanProduction.GreenBeanWeight > 0)
            {
                entity.GreenBeanWeight = roastedBeanProduction.GreenBeanWeight;
            }

            if (roastedBeanProduction.RoastedBeanWeight > 0)
            {
                entity.RoastedBeanWeight = roastedBeanProduction.RoastedBeanWeight;
            }

            if (roastedBeanProduction.IsCancelled)
            {
                entity.IsCancelled = roastedBeanProduction.IsCancelled;
            }
            
            if (roastedBeanProduction.IsFinalized)
            {
                entity.IsFinalized = roastedBeanProduction.IsFinalized;
            }

            DbContext.RoastedBeanProductions.Update(entity);
            return entity;
        }
    }
}
