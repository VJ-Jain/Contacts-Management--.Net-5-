using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clients.Api.Models
{
    public class Client
    {
        [Key]
        public Guid ClientId { get; set; } = Guid.NewGuid();

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public ClientType ClientType { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
