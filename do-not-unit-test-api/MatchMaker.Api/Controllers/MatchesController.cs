using MatchMaker.Api.Contracts;
using MatchMaker.Api.Contracts.Dtos;
using MatchMaker.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchesController : ControllerBase
{
    private readonly GameMatcher matcher;

    public MatchesController(GameMatcher matcher)
    {
        this.matcher = matcher;
    }

    // POST api/matches
    [HttpPost]
    [Authorize]
    public async Task<GameMatchResponse> JoinMatch(JoinMatchRequest request)
    {
        var match = await matcher.MatchPlayerAsync(request.PlayerId);
        return match.ToGameMatchResponse();
    }
}