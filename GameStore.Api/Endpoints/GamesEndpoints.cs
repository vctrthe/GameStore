using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    
    private static readonly List<GameDto> Games = [
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

    public static void MapGamesEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        // Get all games
        // GET /games
        group.MapGet("/", () => Games);

        // Get a game
        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>
        {
            var game = Games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(GetGameEndpointName);

        // Create a game
        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                Games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
    
            Games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        // Update a game
        // PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = Games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            Games[index] = new GameDto(
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
        group.MapDelete("/{id}", (int id) =>
        {
            Games.RemoveAll(game => game.Id == id);
    
            return Results.NoContent();
        });
    }
}