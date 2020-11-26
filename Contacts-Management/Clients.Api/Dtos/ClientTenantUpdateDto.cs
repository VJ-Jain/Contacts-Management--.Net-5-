using System;
using System.ComponentModel.DataAnnotations;

namespace Clients.Api.Dtos
{
    public class ClientTenantUpdateDto
    {
        [Required]
        public Guid TenantId { get; set; }
    }
}
