using System.Net.Http.Json;
using FluentAssertions;
using MatchMaker.Api.Contracts.Dtos;
using MatchMaker.Api.Entities;

namespace MatchMaker.Api.IntegrationTests;

public class MatchesControllerTests
{
    [Fact]
    public async Task JoinMatchRequest_AddsPlayerToMatch()
    {
        // Arrange
        var application = new MatchMakerWebApplicationFactory();
        JoinMatchRequest request = new("P1");

        var client = application.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/matches", request);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299

        var matchResponse = await response.Content.ReadFromJsonAsync<GameMatchResponse>();
        matchResponse?.Id.Should().BePositive();
        matchResponse?.Player1.Should().Be("P1");
        matchResponse?.State.Should().Be(nameof(GameMatchState.WaitingForOpponent));
    }
}