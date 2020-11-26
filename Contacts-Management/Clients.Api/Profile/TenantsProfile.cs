using Clients.Api.Dtos;
using Clients.Api.Models;

namespace Clients.Api.Profile
{
    public class TenantsProfile : AutoMapper.Profile
    {
        public TenantsProfile()
        {
            CreateMap<Tenant, TenantReadDto>();
        }
    }
}
