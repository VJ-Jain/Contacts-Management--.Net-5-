using Clients.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clients.Api.Data
{
    public interface IClientsRepository
    {
        Task<Client> GetClientById(Guid clientId, bool trackChanges = false);
        Task<IEnumerable<Client>> GetAllClientsForTenant(Guid tenantId);
        Task<Client> CreateClient(Client client);
        Task<Client> UpdateClient(Client client);
        Task DeleteClient(Guid clientId);
    }
}
