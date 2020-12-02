using Clients.Api.Controllers;
using Clients.Api.ExternalServices;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Client.Api.Tests.UnitTests
{
    public class ExternalApiControllerTests
    {
        [Fact]
        public async Task Should_PredictAgeFromName()
        {
            // Arrange
            var options = Options.Create<AgeApiConfiguration>(new AgeApiConfiguration { BaseUrl = "anyBaseUrl" });           
            var mockAgeApi = new Mock<AgeApi>(options);
            mockAgeApi.Setup(_ => _.GetAge(It.IsAny<string>())).ReturnsAsync(25);

            var externalApiController = new ExternalApiController(mockAgeApi.Object);

            // Act
            var response = (await externalApiController.PredictAgeFromName("any")).Result as OkObjectResult;

            // Assert
            response.Value.Should().Be("Predicted age for any = 25");
            mockAgeApi.Verify(_ => _.GetAge(It.IsAny<string>()), Times.Once);
        }
    }
}
