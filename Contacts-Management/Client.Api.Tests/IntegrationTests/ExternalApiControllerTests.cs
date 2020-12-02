using Clients.Api;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Client.Api.Tests.IntegrationTests
{
    public class ExternalApiControllerTests
    {
        [Fact]
        public async Task Should_PredictAgeFromName()
        {
            // Arrange
            var customHttpClient = new CustomWebApplicationFactory<Startup>(Guid.NewGuid().ToString()).CreateClient();

            // Act
            var getResponse = await customHttpClient.GetAsync($"/api/predictAge?name=any");

            // Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getResponseBody = await getResponse.Content.ReadAsStringAsync();
            getResponseBody.Should().Be("Predicted age for any = 36");
        }
    }
}
