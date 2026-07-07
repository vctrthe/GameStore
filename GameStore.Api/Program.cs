using GameStore.Api.Dtos;

const string getGameEndpointName = "GetGame";
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
    new (
        1,
        "Street Fighter I",
        "Fighting",
        19.99M,
        new DateOnly(1992, 1, 15)
    ),
    new (
        2,
        "Street Fighter II",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new (
        3,
        "Street Fighter III",
        "Fighting",
        39.99M,
        new DateOnly(1993, 7, 15)
    )
];

// Get all games
// GET /games
app.MapGet("/games", () => games);

// Get a game
// GET /games/{id}
app.MapGet("/games/{id}", (int id) =>
{
    var game = games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);
}).WithName(getGameEndpointName);

// Create a game
// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    
    games.Add(game);

    return Results.CreatedAtRoute(getGameEndpointName, new { id = game.Id }, game);
});

// Update a game
// PUT /games/{id}
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    if (index == -1)
    {
        return Results.NotFound();
    }

    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

// Delete a game
// DELETE /games/{id}
app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);
    
    return Results.NoContent();
});

app.Run();