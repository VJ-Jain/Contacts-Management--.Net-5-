using Clients.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clients.Api.Data
{
    public interface ITenantsRepository
    {
        Task<IEnumerable<Tenant>> GetAllTenants();
        Task<Tenant> GetTenantById(Guid tenantId);
        Task<Tenant> CreateTenant();
        Task DeleteTenant(Guid tenantId);
    }
}
