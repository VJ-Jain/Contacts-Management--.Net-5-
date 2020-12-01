using Clients.Api;
using Clients.Api.Dtos;
using Clients.Api.Models;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Client.Api.Tests.IntegrationTests
{
    public class ClientsControllerTests
    {
        [Fact]
        public async Task Should_CreateClient()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createTenantResponse = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));
            var createTenantResponseBody = await createTenantResponse.Content.ReadAsStringAsync();
            var createdTenant = JsonConvert.DeserializeObject<TenantReadDto>(createTenantResponseBody);

            var client = new ClientCreateDto
            {
                TenantId = createdTenant.TenantId,
                FirstName = "any",
                LastName = "any",
                ContactNumber = "any",
                EmailAddress = "any",
                Country = "any",
                ClientType = ClientType.Free.ToString()
            };
            var payload = JsonConvert.SerializeObject(client);

            // Act
            var createClientResponse = await customHttpClient.PostAsync("/api/clients", new StringContent(payload, Encoding.UTF8, "application/json"));

            // Assert
            var createClientResponseBody = await createClientResponse.Content.ReadAsStringAsync();
            var createdClient = JsonConvert.DeserializeObject<ClientReadDto>(createClientResponseBody);
            createClientResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            createdClient.TenantId.Should().Be(createdTenant.TenantId);
        }

        [Fact]
        public async Task Should_Get_ClientById()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createTenantResponse = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));
            var createTenantResponseBody = await createTenantResponse.Content.ReadAsStringAsync();
            var createdTenant = JsonConvert.DeserializeObject<TenantReadDto>(createTenantResponseBody);

            var client = new ClientCreateDto
            {
                TenantId = createdTenant.TenantId,
                FirstName = "any",
                LastName = "any",
                ContactNumber = "any",
                EmailAddress = "any",
                Country = "any",
                ClientType = ClientType.Free.ToString()
            };
            var payload = JsonConvert.SerializeObject(client);

            var createClientResponse = await customHttpClient.PostAsync("/api/clients", new StringContent(payload, Encoding.UTF8, "application/json"));
            var createClientResponseBody = await createClientResponse.Content.ReadAsStringAsync();
            var createdClient = JsonConvert.DeserializeObject<ClientReadDto>(createClientResponseBody);

            // Act
            var getResponse = await customHttpClient.GetAsync($"/api/clients/{createdClient.ClientId}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await getResponse.Content.ReadAsStringAsync();
            var retrievedClient = JsonConvert.DeserializeObject<ClientReadDto>(responseBody);
            retrievedClient.Should().NotBeNull();
            retrievedClient.ClientId.Should().Be(createdClient.ClientId);
        }

        [Fact]
        public async Task Should_DeleteClient()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createTenantResponse = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));
            var createTenantResponseBody = await createTenantResponse.Content.ReadAsStringAsync();
            var createdTenant = JsonConvert.DeserializeObject<TenantReadDto>(createTenantResponseBody);

            var client = new ClientCreateDto
            {
                TenantId = createdTenant.TenantId,
                FirstName = "any",
                LastName = "any",
                ContactNumber = "any",
                EmailAddress = "any",
                Country = "any",
                ClientType = ClientType.Free.ToString()
            };
            var payload = JsonConvert.SerializeObject(client);

            var createClientResponse = await customHttpClient.PostAsync("/api/clients", new StringContent(payload, Encoding.UTF8, "application/json"));
            var createClientResponseBody = await createClientResponse.Content.ReadAsStringAsync();
            var createdClient = JsonConvert.DeserializeObject<ClientReadDto>(createClientResponseBody);

            // Act
            var getResponseBeforeDeletion = await customHttpClient.GetAsync($"/api/clients/{createdClient.ClientId}");

            //Assert
            getResponseBeforeDeletion.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act
            var deleteResponse = await customHttpClient.DeleteAsync($"/api/clients/{createdClient.ClientId}");

            //Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act
            var getResponseAfterDeletion = await customHttpClient.GetAsync($"/api/clients/{createdClient.ClientId}");

            //Assert
            getResponseAfterDeletion.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_UpdateClient()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createTenantResponse = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));
            var createTenantResponseBody = await createTenantResponse.Content.ReadAsStringAsync();
            var createdTenant = JsonConvert.DeserializeObject<TenantReadDto>(createTenantResponseBody);

            var client = new ClientCreateDto
            {
                TenantId = createdTenant.TenantId,
                FirstName = "any",
                LastName = "any",
                ContactNumber = "any",
                EmailAddress = "any",
                Country = "any",
                ClientType = ClientType.Free.ToString()
            };
            var payload = JsonConvert.SerializeObject(client);

            var createClientResponse = await customHttpClient.PostAsync("/api/clients", new StringContent(payload, Encoding.UTF8, "application/json"));
            var createClientResponseBody = await createClientResponse.Content.ReadAsStringAsync();
            var createdClient = JsonConvert.DeserializeObject<ClientReadDto>(createClientResponseBody);

            var updatedClientDto = new ClientUpdateDto
            {
                FirstName = "any",
                LastName = "any",
                ContactNumber = "any",
                EmailAddress = "any",
                Country = "new Country",
                ClientType = ClientType.Subscriber.ToString()
            };
            var updatePayload = JsonConvert.SerializeObject(updatedClientDto);

            // Act
            var updateResponse = await customHttpClient.PutAsync($"/api/clients/{createdClient.ClientId}", new StringContent(updatePayload, Encoding.UTF8, "application/json"));

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponseBody = await updateResponse.Content.ReadAsStringAsync();
            var updatedClient = JsonConvert.DeserializeObject<ClientReadDto>(updateResponseBody);
            updatedClient.Country.Should().Be("new Country");
            updatedClient.ClientType.Should().Be(ClientType.Subscriber.ToString());
            updatedClient.TenantId.Should().Be(createdClient.TenantId);
        }

        [Fact]
        public async Task Should_MoveClientToNewTenant()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createTenantResponse1 = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));
            var createTenantResponseBody1 = await createTenantResponse1.Content.ReadAsStringAsync();
            var createdTenant1 = JsonConvert.DeserializeObject<TenantReadDto>(createTenantResponseBody1);

            var createTenantResponse2 = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));
            var createTenantResponseBody2 = await createTenantResponse2.Content.ReadAsStringAsync();
            var createdTenant2 = JsonConvert.DeserializeObject<TenantReadDto>(createTenantResponseBody2);

            var client = new ClientCreateDto
            {
                TenantId = createdTenant1.TenantId,
                FirstName = "any",
                LastName = "any",
                ContactNumber = "any",
                EmailAddress = "any",
                Country = "any",
                ClientType = ClientType.Free.ToString()
            };
            var payload = JsonConvert.SerializeObject(client);

            var createClientResponse = await customHttpClient.PostAsync("/api/clients", new StringContent(payload, Encoding.UTF8, "application/json"));
            var createClientResponseBody = await createClientResponse.Content.ReadAsStringAsync();
            var createdClient = JsonConvert.DeserializeObject<ClientReadDto>(createClientResponseBody);

            var clientTenantUpdateDto = new ClientTenantUpdateDto
            {
                TenantId = createdTenant2.TenantId
            };
            var patchPayload = JsonConvert.SerializeObject(clientTenantUpdateDto);

            // Act
            var patchResponse = await customHttpClient.PatchAsync($"/api/clients/{createdClient.ClientId}", new StringContent(patchPayload, Encoding.UTF8, "application/json"));

            // Assert
            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var patchResponseBody = await patchResponse.Content.ReadAsStringAsync();
            var movedToNewTenantClient = JsonConvert.DeserializeObject<ClientReadDto>(patchResponseBody);
            movedToNewTenantClient.TenantId.Should().Be(createdTenant2.TenantId);
        }
    }
}
