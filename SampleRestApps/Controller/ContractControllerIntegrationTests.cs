// Controller/ContractControllerIntegrationTests.cs
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SampleRestApps.Models;
using SampleRestApps.Services;
using Xunit;

namespace SampleRestApps.Controller;

public class ContractControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ContractControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private (HttpClient client, Mock<IContractService> service) CreateClient()
    {
        var mock = new Mock<IContractService>();

        var client = _factory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
                services.AddScoped<IContractService>(_ => mock.Object)))
            .CreateClient();

        return (client, mock);
    }

    [Fact]
    public async Task UploadContract_Returns200()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.UploadContractAsync("t1", "c1", It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

        var response = await client.PostAsync("/api/v1/tenants/t1/contracts/c1/upload", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UploadContract_WhenDownstreamReturnsBadRequest_Returns400()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.UploadContractAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
               .ThrowsAsync(new ApiException("bad request", HttpStatusCode.BadRequest));

        var response = await client.PostAsync("/api/v1/tenants/t1/contracts/c1/upload", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UploadContract_WhenContractNotFound_Returns404()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.UploadContractAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
               .ThrowsAsync(new ApiException("not found", HttpStatusCode.NotFound));

        var response = await client.PostAsync("/api/v1/tenants/t1/contracts/c1/upload", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UploadContract_WhenDownstreamUnavailable_Returns502()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.UploadContractAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
               .ThrowsAsync(new ApiException("upstream down", HttpStatusCode.ServiceUnavailable));

        var response = await client.PostAsync("/api/v1/tenants/t1/contracts/c1/upload", null);

        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
    }
}