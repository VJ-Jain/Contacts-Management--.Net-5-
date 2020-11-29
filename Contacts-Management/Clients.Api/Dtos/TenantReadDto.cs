using System;

namespace Clients.Api.Dtos
{
    public class TenantReadDto
    {
        public Guid TenantId { get; set; }
        public String DateCreated { get; set; }
    }
}
