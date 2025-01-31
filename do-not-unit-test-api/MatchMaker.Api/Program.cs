using MatchMaker.Api.Data;
using MatchMaker.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositories(builder.Configuration)
                .AddScoped<GameMatcher>();

builder.Services.AddControllers();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

await app.Services.InitializeDbAsync();

app.MapControllers();

app.Run();