using AutoMapper;
using Clients.Api.Data;
using Clients.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clients.Api.Controllers
{
    [ApiController]
    [Route("/api/tenants")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantsRepository _tenantsRepo;
        private readonly IClientsRepository _clientsRepo;
        private readonly IMapper _mapper;

        public TenantsController(ITenantsRepository tenantsRepo, IClientsRepository clientsRepo, IMapper mapper)
        {
            _tenantsRepo = tenantsRepo;
            _clientsRepo = clientsRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TenantReadDto>>> GetAllTenants()
        {
            return Ok(_mapper.Map<IEnumerable<TenantReadDto>>(await _tenantsRepo.GetAllTenants()));
        }

        [HttpGet("{tenantId}", Name = "GetTenantById")]
        public async Task<ActionResult<TenantReadDto>> GetTenantById(Guid tenantId)
        {
            var tenantFromRepo = await _tenantsRepo.GetTenantById(tenantId);
            if (tenantFromRepo == null)
                return NotFound();

            return Ok(_mapper.Map<TenantReadDto>(tenantFromRepo));
        }

        [HttpGet]
        [Route("{tenantId}/clients")]
        public async Task<ActionResult<IEnumerable<ClientReadDto>>> GetAllClientsForATenant(Guid tenantId)
        {
            var tenantFromRepo = await _tenantsRepo.GetTenantById(tenantId);
            if (tenantFromRepo == null)
                return NotFound();

            var allClientsForTenant = await _clientsRepo.GetAllClientsForTenant(tenantId);
            return Ok(_mapper.Map<IEnumerable<ClientReadDto>>(allClientsForTenant));
        }

        [HttpPost]
        public async Task<ActionResult<TenantReadDto>> CreateTenant()
        {
            var newTenant = await _tenantsRepo.CreateTenant();
            return CreatedAtRoute(nameof(GetTenantById), new { newTenant.TenantId }, _mapper.Map<TenantReadDto>(newTenant));
        }

        [HttpDelete]
        [Route("{tenantId}")]
        public async Task<ActionResult> DeleteTenant(Guid tenantId)
        {
            var tenantFromRepo = await _tenantsRepo.GetTenantById(tenantId);
            if (tenantFromRepo == null)
                return NotFound();

            await _tenantsRepo.DeleteTenant(tenantId);
            return NoContent();
        }
    }
}
