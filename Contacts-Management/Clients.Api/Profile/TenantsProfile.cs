using Clients.Api.Dtos;
using Clients.Api.Models;
using System;

namespace Clients.Api.Profile
{
    public class TenantsProfile : AutoMapper.Profile
    {
        public TenantsProfile()
        {
            CreateMap<Tenant, TenantReadDto>()
                .ForMember(d => d.DateCreated, s => s.MapFrom(o => o.DateCreated.ToShortDateString()));
        }
    }
}
