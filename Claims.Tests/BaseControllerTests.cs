using System.Text.Json;
using System.Text.Json.Serialization;

namespace Claims.Tests;

public class BaseControllerTests
{
    protected readonly JsonSerializerOptions? SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(jsonResponse, SerializerOptions);
    }
}