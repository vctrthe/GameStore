using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddValidation();
builder.AddGameStoreDb();
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Authority = "http://keycloak.gamestore.orb.local/realms/gamestore";
    options.Audience = "gamestore-api";
    // Only for development
    options.RequireHttpsMetadata = false;
});

builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapGamesEndpoint();
app.MapGenresEndpoints();

app.MigrateDb();

app.Run();