using System.Text.Json;

namespace SampleRestApps.Models;

public static class ApiExceptionExtensions
{
    public static DownstreamErrorResponse? TryDeserializeError(this ApiException ex)
    {
        if (string.IsNullOrWhiteSpace(ex.Response))
            return null;

        try
        {
            return JsonSerializer.Deserialize<DownstreamErrorResponse>(ex.Response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch
        {
            return null;
        }
    }
}