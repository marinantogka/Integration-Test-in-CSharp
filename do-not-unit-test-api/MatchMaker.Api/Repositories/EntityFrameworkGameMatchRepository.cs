using MatchMaker.Api.Data;
using MatchMaker.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatchMaker.Api.Repositories;

public class EntityFrameworkGameMatchRepository : IGameMatchRepository
{
    private readonly MatchMakerDbContext dbContext;

    public EntityFrameworkGameMatchRepository(MatchMakerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<GameMatch?> FindMatchForPlayerAsync(string playerId)
    {
        return await dbContext.Matches
                              .Where(match => match.Player1 == playerId || match.Player2 == playerId)
                              .AsNoTracking()
                              .FirstOrDefaultAsync();
    }

    public async Task<GameMatch?> FindOpenMatchAsync()
    {
        return await dbContext.Matches
                              .Where(match => match.State == GameMatchState.WaitingForOpponent)
                              .AsNoTracking()
                              .FirstOrDefaultAsync();
    }

    public async Task CreateMatchAsync(GameMatch match)
    {
        dbContext.Matches.Add(match);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateMatchAsync(GameMatch match)
    {
        dbContext.Update(match);
        await dbContext.SaveChangesAsync();
    }
}