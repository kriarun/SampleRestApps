// Controller/DocumentControllerIntegrationTests.cs
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SampleRestApps.Models;
using SampleRestApps.Services;
using Xunit;

namespace SampleRestApps.Controller;

public class DocumentControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DocumentControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private (HttpClient client, Mock<IDocumentService> service) CreateClient()
    {
        var mock = new Mock<IDocumentService>();

        var client = _factory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
                services.AddScoped<IDocumentService>(_ => mock.Object)))
            .CreateClient();

        return (client, mock);
    }

    private static LetterRequest ValidRequest() =>
        new() { ActivationCodes = ["CODE1", "CODE2"] };

    // OneLetter tests

    [Fact]
    public async Task SendOneLetter_Returns200_WithResponse()
    {
        var (client, service) = CreateClient();
        var expected = new LetterResponse { IsPrinted = true, Message = "sent" };

        service.Setup(s => s.SendLetterAsync("t1", "c1", It.IsAny<LetterRequest>(),
                    LetterType.OneLetter, It.IsAny<CancellationToken>()))
               .ReturnsAsync(expected);

        var response = await client.PostAsJsonAsync(
            "/api/v1/tenants/t1/documents/c1/one-letter", ValidRequest());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<LetterResponse>();
        Assert.True(result?.IsPrinted);
    }

    [Fact]
    public async Task SendOneLetter_WhenDownstreamReturnsBadRequest_Returns400()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.SendLetterAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<LetterRequest>(), LetterType.OneLetter, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new ApiException("bad request", HttpStatusCode.BadRequest));

        var response = await client.PostAsJsonAsync(
            "/api/v1/tenants/t1/documents/c1/one-letter", ValidRequest());

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SendOneLetter_WhenLetterNotFound_Returns404()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.SendLetterAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<LetterRequest>(), LetterType.OneLetter, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new ApiException("not found", HttpStatusCode.NotFound));

        var response = await client.PostAsJsonAsync(
            "/api/v1/tenants/t1/documents/c1/one-letter", ValidRequest());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task SendOneLetter_WhenDownstreamUnavailable_Returns502()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.SendLetterAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<LetterRequest>(), LetterType.OneLetter, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new ApiException("upstream down", HttpStatusCode.ServiceUnavailable));

        var response = await client.PostAsJsonAsync(
            "/api/v1/tenants/t1/documents/c1/one-letter", ValidRequest());

        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
    }

    // TwoLetters tests

    [Fact]
    public async Task SendTwoLetters_Returns200_WithResponse()
    {
        var (client, service) = CreateClient();
        var expected = new LetterResponse { IsPrinted = true, Message = "sent" };

        service.Setup(s => s.SendLetterAsync("t1", "c1", It.IsAny<LetterRequest>(),
                    LetterType.TwoLetters, It.IsAny<CancellationToken>()))
               .ReturnsAsync(expected);

        var response = await client.PostAsJsonAsync(
            "/api/v1/tenants/t1/documents/c1/two-letters", ValidRequest());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<LetterResponse>();
        Assert.True(result?.IsPrinted);
    }

    [Fact]
    public async Task SendTwoLetters_WhenDownstreamUnavailable_Returns502()
    {
        var (client, service) = CreateClient();

        service.Setup(s => s.SendLetterAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<LetterRequest>(), LetterType.TwoLetters, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new ApiException("upstream down", HttpStatusCode.ServiceUnavailable));

        var response = await client.PostAsJsonAsync(
            "/api/v1/tenants/t1/documents/c1/two-letters", ValidRequest());

        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
    }
}