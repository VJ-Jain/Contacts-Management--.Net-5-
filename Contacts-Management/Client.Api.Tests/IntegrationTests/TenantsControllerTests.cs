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
        public static async Task<TenantReadDto> CreateTenant(HttpClient httpClient)
        {
            var createResponse = await httpClient.PostAsync("/api/tenants", new StringContent(""));
            var createResponseBody = await createResponse.Content.ReadAsStringAsync();
            var createdTenant = JsonConvert.DeserializeObject<TenantReadDto>(createResponseBody);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            createdTenant.Should().NotBeNull();

            return createdTenant;
        }        
        
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

            var createdTenant1 = await CreateTenant(customHttpClient);
            var createdTenant2 = await CreateTenant(customHttpClient);

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

            // Act, Assert
            await CreateTenant(customHttpClient);
        }

        [Fact]
        public async Task Should_Get_Tenant_ById()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createdTenant1 = await CreateTenant(customHttpClient);
            var createdTenant2 = await CreateTenant(customHttpClient);

            // Act
            var getResponse = await customHttpClient.GetAsync($"/api/tenants/{createdTenant1.TenantId}");

            //Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await getResponse.Content.ReadAsStringAsync();
            var retrievedTenant = JsonConvert.DeserializeObject<TenantReadDto>(responseBody);
            retrievedTenant.Should().NotBeNull();
            retrievedTenant.TenantId.Should().Be(createdTenant1.TenantId);
        }

        [Fact]
        public async Task Should_Delete_Tenant()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createdTenant1 = await CreateTenant(customHttpClient);
            var createdTenant2 = await CreateTenant(customHttpClient);

            // Act
            var getResponseBeforeDeletion = await customHttpClient.GetAsync($"/api/tenants/{createdTenant1.TenantId}");

            //Assert
            getResponseBeforeDeletion.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act
            var deleteResponse = await customHttpClient.DeleteAsync($"/api/tenants/{createdTenant1.TenantId}");

            //Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Act
            var getResponseAfterDeletion = await customHttpClient.GetAsync($"/api/tenants/{createdTenant1.TenantId}");

            //Assert
            getResponseAfterDeletion.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_Get_All_Clients_For_A_Tenant()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            var createdTenant = await CreateTenant(customHttpClient);
            var createdClient = await ClientsControllerTests.CreateClient(customHttpClient, createdTenant.TenantId);

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
