using Clients.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clients.Api.Data
{
    public class SqlClientsRepository : IClientsRepository
    {
        private readonly ClientsApiDbContext _dbContext;

        public SqlClientsRepository(ClientsApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Client> CreateClient(Client client)
        {
            _dbContext.Clients.Add(client);
            await _dbContext.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateClient(Client client)
        {
            await _dbContext.SaveChangesAsync();
            return client;
        }

        public async Task DeleteClient(Guid clientId)
        {
            var client = _dbContext.Clients.FirstOrDefault(_ => _.IsDeleted == false && _.ClientId == clientId);
            if (client != null)
            {
                client.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Client> GetClientById(Guid clientId, bool trackChanges = false)
        {
            if (trackChanges)
                return await _dbContext.Clients.FirstOrDefaultAsync(_ => _.IsDeleted == false && _.ClientId == clientId);

            return await _dbContext.Clients.AsNoTracking().FirstOrDefaultAsync(_ => _.IsDeleted == false && _.ClientId == clientId);
        }

        public async Task<IEnumerable<Client>> GetAllClientsForTenant(Guid tenantId)
        {
            return await _dbContext.Clients.AsNoTracking().Where(_ => _.IsDeleted == false && _.TenantId == tenantId).ToListAsync();
        }
    }
}
