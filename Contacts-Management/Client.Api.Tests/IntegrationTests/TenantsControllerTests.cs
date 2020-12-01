using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Clients.Api.Dtos;
using Newtonsoft.Json;
using System;
using Clients.Api;
using Clients.Api.Models;
using System.Text;

namespace Client.Api.Tests.IntegrationTests
{
    public class TenantsControllerTests
    {
        [Fact]
        public async Task Should_Get_Zero_Tenants_When_Not_Created()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            // Act
            var getResponse = await customHttpClient.GetAsync("/api/tenants");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await getResponse.Content.ReadAsStringAsync();
            var tenantList = JsonConvert.DeserializeObject<List<TenantReadDto>>(responseBody);
            tenantList.Count.Should().Be(0);
        }

        [Fact]
        public async Task Should_Get_All_Tenants_After_Creation()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createResponse1 = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));
            var createResponseBody1 = await createResponse1.Content.ReadAsStringAsync();
            var createdTenant1 = JsonConvert.DeserializeObject<TenantReadDto>(createResponseBody1);

            var createResponse2 = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));
            var createResponseBody2 = await createResponse2.Content.ReadAsStringAsync();
            var createdTenant2 = JsonConvert.DeserializeObject<TenantReadDto>(createResponseBody2);

            // Act
            var getResponse = await customHttpClient.GetAsync("/api/tenants");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await getResponse.Content.ReadAsStringAsync();
            var retrievedTenantList = JsonConvert.DeserializeObject<List<TenantReadDto>>(responseBody);
            retrievedTenantList.Count.Should().Be(2);
            retrievedTenantList.Find(_ => _.TenantId == createdTenant1.TenantId).Should().NotBeNull();
            retrievedTenantList.Find(_ => _.TenantId == createdTenant2.TenantId).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Create_Tenant()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            // Act
            var createResponse = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));

            //Assert
            var createResponseBody = await createResponse.Content.ReadAsStringAsync();
            var createdTenant = JsonConvert.DeserializeObject<TenantReadDto>(createResponseBody);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            createdTenant.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Get_Tenant_ById()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createResponse = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));
            var createResponseBody = await createResponse.Content.ReadAsStringAsync();
            var createdTenant = JsonConvert.DeserializeObject<TenantReadDto>(createResponseBody);

            await customHttpClient.PostAsync("/api/tenants", new StringContent(""));

            // Act
            var getResponse = await customHttpClient.GetAsync($"/api/tenants/{createdTenant.TenantId}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await getResponse.Content.ReadAsStringAsync();
            var retrievedTenant = JsonConvert.DeserializeObject<TenantReadDto>(responseBody);
            retrievedTenant.Should().NotBeNull();
            retrievedTenant.TenantId.Should().Be(createdTenant.TenantId);
        }

        [Fact]
        public async Task Should_Delete_Tenant()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createResponse = await customHttpClient.PostAsync("/api/tenants", new StringContent(""));
            var createResponseBody = await createResponse.Content.ReadAsStringAsync();
            var createdTenant = JsonConvert.DeserializeObject<TenantReadDto>(createResponseBody);

            await customHttpClient.PostAsync("/api/tenants", new StringContent(""));

            // Act
            var getResponseBeforeDeletion = await customHttpClient.GetAsync($"/api/tenants/{createdTenant.TenantId}");

            //Assert
            getResponseBeforeDeletion.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act
            var deleteResponse = await customHttpClient.DeleteAsync($"/api/tenants/{createdTenant.TenantId}");

            //Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act
            var getResponseAfterDeletion = await customHttpClient.GetAsync($"/api/tenants/{createdTenant.TenantId}");

            //Assert
            getResponseAfterDeletion.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_Get_All_Clients_For_A_Tenant()
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
            var getAllClientsForTenantResponse = await customHttpClient.GetAsync($"/api/tenants/{createdTenant.TenantId}/clients");

            //Assert
            getAllClientsForTenantResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getAllClientsForTenantResponseBody = await getAllClientsForTenantResponse.Content.ReadAsStringAsync();
            var retrievedClientsList = JsonConvert.DeserializeObject<List<ClientReadDto>>(getAllClientsForTenantResponseBody);

            retrievedClientsList.Count.Should().Be(1);
            retrievedClientsList[0].ClientId.Should().Be(createdClient.ClientId);
        }
    }
}
