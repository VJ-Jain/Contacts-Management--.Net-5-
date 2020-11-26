using System;
using System.ComponentModel.DataAnnotations;

namespace Clients.Api.Dtos
{
    public class ClientUpdateDto
    {
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
        public String ClientType { get; set; }
    }
}
