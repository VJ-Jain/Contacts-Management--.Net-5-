using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clients.Api.Models
{
    public class Tenant
    {
        [Key]
        public Guid TenantId { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required]
        public bool IsDeleted { get; set; } = false;

        public IEnumerable<Client> Clients { get; set; }
    }
}
