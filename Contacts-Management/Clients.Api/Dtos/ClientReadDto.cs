using Clients.Api.Models;
using System;

namespace Clients.Api.Dtos
{
    public class ClientReadDto
    {
        public Guid ClientId { get; set; }
        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Country { get; set; }
        public ClientType ClientType { get; set; }
    }
}
