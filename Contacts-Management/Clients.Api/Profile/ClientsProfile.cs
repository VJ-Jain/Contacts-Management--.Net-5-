using Clients.Api.Dtos;
using Clients.Api.Models;

namespace Clients.Api.Profile
{
    public class ClientsProfile : AutoMapper.Profile
    {
        public ClientsProfile()
        {
            CreateMap<Client, ClientReadDto>();
            CreateMap<ClientCreateDto, Client>();
            CreateMap<ClientUpdateDto, Client>();
            CreateMap<ClientTenantUpdateDto, Client>();
        }
    }
}
