using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var app = builder.Build();

app.MapGamesEndpoint();

app.Run();