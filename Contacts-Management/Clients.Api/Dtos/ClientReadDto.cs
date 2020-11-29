using Clients.Api.Models;
using System;

namespace Clients.Api.Dtos
{
    public class ClientReadDto
    {
        public Guid ClientId { get; set; }
        public Guid TenantId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String ContactNumber { get; set; }
        public String EmailAddress { get; set; }
        public String Country { get; set; }
        public String ClientType { get; set; }
    }
}
