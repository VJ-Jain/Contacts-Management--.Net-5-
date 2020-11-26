using System;

namespace Clients.Api.Dtos
{
    public class TenantReadDto
    {
        public Guid TenantId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
