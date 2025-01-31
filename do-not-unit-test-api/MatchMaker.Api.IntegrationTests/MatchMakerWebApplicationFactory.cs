using MatchMaker.Api.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MatchMaker.Api.IntegrationTests;

internal class MatchMakerWebApplicationFactory : WebApplicationFactory<Program>
{
    override protected void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove the existing DbContextOptions
            services.RemoveAll(typeof(DbContextOptions<MatchMakerDbContext>));

            // Register a new DBContext that will use our test connection string
            string? connString = GetConnectionString();
            services.AddSqlServer<MatchMakerDbContext>(connString);

            // Add the authentication handler
            services.AddAuthentication(defaultScheme: "TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", options => { });

            // Delete the database (if exists) to ensure we start clean
            MatchMakerDbContext dbContext = CreateDbContext(services);
            dbContext.Database.EnsureDeleted();
        });
    }

    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<MatchMakerWebApplicationFactory>()
            .Build();

        var connString = configuration.GetConnectionString("MatchMaker");
        return connString;
    }

    private static MatchMakerDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MatchMakerDbContext>();
        return dbContext;
    }    
}