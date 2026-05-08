
// Controller/ActivationCodeControllerIntegrationTests.cs
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;

namespace SampleRestApps.Controller;


public class ActivationCodeControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ActivationCodeControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private (HttpClient client, Mock<IActivationCodeService> service) CreateClient()
    {
        var mock = new Mock<IActivationCodeService>();

        var client = _factory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
                services.AddScoped<IActivationCodeService>(_ => mock.Object)))
            .CreateClient();

        return (client, mock);
    }

    [Fact]
    public async Task SendActivationCodes_Returns200_WithValidRequest()
    {
        var (client, service) = CreateClient();
        var expected = new ActivationCodeResponse { Id = "1", ActivationCode = "ABC123" };

        service.Setup(s => s.SendActivationCodesAsync("t1", "c1", It.IsAny<ActivationCodeRequest>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(expected);

        var response = await client.PostAsJsonAsync(
            "/api/v1/tenants/t1/contracts/c1/activation-codes",
            new ActivationCodeRequest { ValidTo = DateTime.UtcNow.AddDays(1), MetaData = "m", Purpose = "p", Length = 8 });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ActivationCodeResponse>();
        Assert.Equal("ABC123", result?.ActivationCode);
    }

    [Fact]
    public async Task SendActivationCodes_WhenDownstreamReturns400_Returns400()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.SendActivationCodesAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<ActivationCodeRequest>(), It.IsAny<CancellationToken>()))
               .ThrowsAsync(new ApiException("bad input", HttpStatusCode.BadRequest));

        var response = await client.PostAsJsonAsync(
            "/api/v1/tenants/t1/contracts/c1/activation-codes",
            new ActivationCodeRequest());

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SendActivationCodes_WhenDownstreamReturns404_Returns404()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.SendActivationCodesAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<ActivationCodeRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ApiException("not found", HttpStatusCode.NotFound));

        var response = await client.PostAsJsonAsync(
            "/api/v1/tenants/t1/contracts/c1/activation-codes",
            new ActivationCodeRequest { ValidTo = DateTime.UtcNow.AddDays(1), MetaData = "m", Purpose = "p", Length = 8 });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task SendActivationCodes_WhenDownstreamReturns503_Returns502()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.SendActivationCodesAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<ActivationCodeRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ApiException("upstream down", HttpStatusCode.ServiceUnavailable));

        var response = await client.PostAsJsonAsync(
            "/api/v1/tenants/t1/contracts/c1/activation-codes",
            new ActivationCodeRequest { ValidTo = DateTime.UtcNow.AddDays(1), MetaData = "m", Purpose = "p", Length = 8 });

        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
    }

    [Fact]
    public async Task DeleteActivationCodes_Returns204()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.DeleteActivationCodesAsync("t1", "c1", It.IsAny<CancellationToken>()))
               .Returns(Task.CompletedTask);

        var response = await client.DeleteAsync("/api/v1/tenants/t1/contracts/c1/activation-codes");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteActivationCodes_WhenDownstreamReturns404_Returns404()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.DeleteActivationCodesAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
               .ThrowsAsync(new ApiException("not found", HttpStatusCode.NotFound));

        var response = await client.DeleteAsync("/api/v1/tenants/t1/contracts/c1/activation-codes");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}