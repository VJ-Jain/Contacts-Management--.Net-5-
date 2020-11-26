using AutoMapper;
using Clients.Api.Data;
using Clients.Api.Dtos;
using Clients.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Clients.Api.Controllers
{
    [ApiController]
    [Route("/api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsRepository _clientsRepo;
        private readonly ITenantsRepository _tenantsRepo;
        private readonly IMapper _mapper;

        public ClientsController(IClientsRepository clientsRepo, ITenantsRepository tenantsRepo, IMapper mapper)
        {
            _clientsRepo = clientsRepo;
            _tenantsRepo = tenantsRepo;
            _mapper = mapper;
        }

        [HttpGet("{clientId}", Name = "GetClientById")]
        public async Task<ActionResult<ClientReadDto>> GetClientById(Guid clientId)
        {
            var clientFromRepo = await _clientsRepo.GetClientById(clientId);
            if (clientFromRepo == null)
                return NotFound();

            return Ok(_mapper.Map<ClientReadDto>(clientFromRepo));
        }

        [HttpPost]
        public async Task<ActionResult<ClientReadDto>> CreateClient(ClientCreateDto clientCreateDto)
        {
            if (await _tenantsRepo.GetTenantById(clientCreateDto.TenantId) == null)
                return BadRequest(new { Error = "Invalid Tenant Id" });

            var clientToAdd = _mapper.Map<Client>(clientCreateDto);
            var newClient = await _clientsRepo.CreateClient(clientToAdd);
            return CreatedAtRoute(nameof(GetClientById), new { newClient.ClientId }, _mapper.Map<ClientReadDto>(newClient));
        }

        [HttpDelete]
        [Route("{clientId}")]
        public async Task<ActionResult> DeleteClient(Guid clientId)
        {
            var clientFromRepo = await _clientsRepo.GetClientById(clientId);
            if (clientFromRepo == null)
                return NotFound();

            await _clientsRepo.DeleteClient(clientId);
            return NoContent();
        }

        [HttpPut]
        [Route("{clientId}")]
        public async Task<ActionResult<ClientReadDto>> UpdateClient(Guid clientId, ClientUpdateDto clientUpdateDto)
        {
            var client = await _clientsRepo.GetClientById(clientId, true);
            if (client == null)
                return NotFound();

            client = _mapper.Map(clientUpdateDto, client);
            var updatedClient = await _clientsRepo.UpdateClient(client);
            return Ok(_mapper.Map<ClientReadDto>(updatedClient));
        }

        [HttpPatch]
        [Route("{clientId}")]
        public async Task<ActionResult<ClientReadDto>> MoveClientToNewTenant(Guid clientId, ClientTenantUpdateDto clientTenantUpdateDto)
        {
            var client = await _clientsRepo.GetClientById(clientId, true);
            if (client == null)
                return NotFound();

            if (await _tenantsRepo.GetTenantById(clientTenantUpdateDto.TenantId) == null)
                return BadRequest(new { Error = "Invalid Tenant Id" });

            client = _mapper.Map(clientTenantUpdateDto, client);
            var updatedClient = await _clientsRepo.UpdateClient(client);
            return Ok(_mapper.Map<ClientReadDto>(updatedClient));
        }
    }
}
