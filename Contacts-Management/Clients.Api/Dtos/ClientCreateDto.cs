using System;
using System.ComponentModel.DataAnnotations;

namespace Clients.Api.Dtos
{
    public class ClientCreateDto
    {
        [Required]
        public Guid TenantId { get; set; }

        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }

        [Required]
        public String ContactNumber { get; set; }

        [Required]
        public String EmailAddress { get; set; }

        [Required]
        public String Country { get; set; }

        [Required]
        public String ClientType { get; set; }
    }
}
