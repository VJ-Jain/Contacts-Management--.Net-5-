using Clients.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clients.Api.Data
{
    public class SqlTenantsRepository : ITenantsRepository
    {
        private readonly ClientsApiDbContext _dbContext;

        public SqlTenantsRepository(ClientsApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Tenant>> GetAllTenants()
        {
            return await _dbContext.Tenants.AsNoTracking().Where(_ => _.IsDeleted == false).ToListAsync();
        }

        public async Task<Tenant> GetTenantById(Guid tenantId)
        {
            return await _dbContext.Tenants.AsNoTracking().FirstOrDefaultAsync(_ => _.IsDeleted == false && _.TenantId == tenantId);
        }

        public async Task<Tenant> CreateTenant()
        {
            var tenant = new Tenant();
            _dbContext.Tenants.Add(tenant);
            await _dbContext.SaveChangesAsync();
            return tenant;
        }

        public async Task DeleteTenant(Guid tenantId)
        {
            var tenant = _dbContext.Tenants.FirstOrDefault(_ => _.IsDeleted == false && _.TenantId == tenantId);
            if (tenant != null)
            {
                tenant.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
