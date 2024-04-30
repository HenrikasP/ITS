using System.Net;
using System.Net.Http.Json;
using Claims.Contracts.Requests;
using Claims.Tests.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Claims.Tests;

public class ClaimsControllerTests : BaseControllerTests
{
    private readonly HttpClient _client;

    public ClaimsControllerTests()
    {
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        _client = application.CreateClient();
    }

    [Theory]
    [InlineAutoMoqData(-1, "Name")]
    [InlineAutoMoqData(0, "Name")]
    [InlineAutoMoqData(100001, "Name")]
    [InlineAutoMoqData(200000, "Name")]
    [InlineAutoMoqData(200000, "Name")]
    [InlineAutoMoqData(200000, "Name")]
    [InlineAutoMoqData(100, "")]
    [InlineAutoMoqData(100, null)]
    public async Task CreateClaims_ShouldFail_IfInvalidDataIsProvided(decimal damageCost, string name, Guid id)
    {
        // Arrange
        // Act
        var request = new CreateClaimRequest
        {
            CoverId = id,
            Name = name,
            DamageCost = damageCost
        };
        var response = await _client.PostAsJsonAsync("/v1/Claims", request);
        
        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [CustomAutoData]
    public async Task CreateClaims_ShouldFail_IfCoverWasNotFound(Guid id, string name)
    {
        // Arrange
        // Act
        var request = new CreateClaimRequest
        {
            CoverId = id,
            Name = name,
            DamageCost = 1000
        };
        var response = await _client.PostAsJsonAsync("/v1/Claims", request);
        
        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_Claims()
    {
        // Arrange
        // Act
        var response = await _client.GetAsync("/v1/Claims");
        
        // Assert
        response.EnsureSuccessStatusCode();
        // TODO add checks for size for result, add check for each of item 
        // var result = await DeserializeResponse<IEnumerable<CoverResponse>>(response);
    }

}