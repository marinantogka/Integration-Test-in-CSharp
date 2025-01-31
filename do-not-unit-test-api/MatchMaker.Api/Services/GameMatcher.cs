using MatchMaker.Api.Entities;
using MatchMaker.Api.Contracts.Dtos;
using MatchMaker.Api.Repositories;
using MatchMaker.Api.Contracts;

namespace MatchMaker.Api.Services;

public class GameMatcher
{
    private readonly IGameMatchRepository repository;
    private readonly ILogger<GameMatcher> logger;

    public GameMatcher(IGameMatchRepository repository, ILogger<GameMatcher> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    public async Task<GameMatch> MatchPlayerAsync(string playerId)
    {
        logger.LogInformation("Matching player {PlayerId}...", playerId);

        // Is the player already assigned to a match?
        GameMatch? match = await repository.FindMatchForPlayerAsync(playerId);

        if (match is null)
        {
            // Is there an open match he can join?
            match = await repository.FindOpenMatchAsync();

            if (match is null)
            {
                // Create a new match
                match = new GameMatch(playerId);

                await repository.CreateMatchAsync(match);
            }
            else
            {
                // Assign to open match
                match.SetPlayer2(playerId);
                await repository.UpdateMatchAsync(match);
            }

            logger.LogInformation("{PlayerId} assigned to match {MatchId}.", playerId, match.Id);
        }
        else
        {
            logger.LogInformation("{PlayerId} already assigned to match {MatchId}.", playerId, match.Id);
        }

        return match;
    }

    public async Task<GameMatchResponse?> GetMatchForPlayerAsync(string playerId)
    {
        var match = await repository.FindMatchForPlayerAsync(playerId);
        return match?.ToGameMatchResponse();
    }    
}
