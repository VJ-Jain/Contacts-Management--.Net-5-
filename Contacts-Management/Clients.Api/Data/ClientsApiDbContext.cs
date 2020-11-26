using Clients.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Clients.Api.Data
{
    public class ClientsApiDbContext : DbContext
    {
        public ClientsApiDbContext(DbContextOptions<ClientsApiDbContext> options) : base(options)
        {

        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}
