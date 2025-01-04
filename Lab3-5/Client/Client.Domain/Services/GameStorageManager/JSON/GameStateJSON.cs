using Client.Domain.Services.GameServices;
using Client.Domain.Services.Settings.GameSettingsService;
using System.Text.Json.Serialization;

namespace Client.Domain.Services.GameStorageManager.JSON;

/// <summary>
/// Serializable representation of the game state, used for JSON storage.
/// </summary>
[Serializable]
public class GameStateJSON
{
    /// <summary>
    /// The game board represented as a nested list of nullable booleans.
    /// </summary>
    [JsonPropertyName("board")]
    public List<List<bool?>> Board { get; set; }

    /// <summary>
    /// The current game mode.
    /// </summary>
    [JsonPropertyName("mode")]
    public GameMode Mode { get; set; }

    /// <summary>
    /// The current status of the game.
    /// </summary>
    [JsonPropertyName("status")]
    public GameStatus Status { get; set; }

    /// <summary>
    /// Indicates which player is the human player, if applicable.
    /// </summary>
    [JsonPropertyName("manPlayer")]
    public bool? ManPlayer { get; set; }

    /// <summary>
    /// Default constructor. Initializes the board as an empty list.
    /// </summary>
    public GameStateJSON()
    {
        Board = new List<List<bool?>>();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="GameStateJSON"/> based on the given <see cref="GameState"/>.
    /// </summary>
    /// <param name="state">The <see cref="GameState"/> to convert into a serializable format.</param>
    public GameStateJSON(GameState state)
    {
        Board = ConvertToNestedList(state.Board);
        Mode = state.Mode;
        Status = state.Status;
        ManPlayer = state.ManPlayer;
    }

    /// <summary>
    /// Converts this <see cref="GameStateJSON"/> instance back into a <see cref="GameState"/> object.
    /// </summary>
    /// <returns>The deserialized <see cref="GameState"/> object.</returns>
    public GameState ToGameState()
    {
        GameState state = new();
        state.Board = ConvertToMultidimensionalArray(Board);
        state.Mode = Mode;
        state.Status = Status;
        state.ManPlayer = ManPlayer;
        return state;
    }

    /// <summary>
    /// Converts a two-dimensional nullable boolean array into a nested list.
    /// </summary>
    /// <param name="array">The two-dimensional array to convert.</param>
    /// <returns>A nested list representing the array.</returns>
    private static List<List<bool?>> ConvertToNestedList(bool?[,] array)
    {
        var list = new List<List<bool?>>();
        for (int i = 0; i < array.GetLength(0); i++)
        {
            var row = new List<bool?>();
            for (int j = 0; j < array.GetLength(1); j++)
            {
                row.Add(array[i, j]);
            }
            list.Add(row);
        }
        return list;
    }

    /// <summary>
    /// Converts a nested list of nullable booleans into a two-dimensional array.
    /// </summary>
    /// <param name="list">The nested list to convert.</param>
    /// <returns>A two-dimensional array representing the list.</returns>
    /// <remarks>
    /// If the input list is empty or contains empty rows, the returned array will have zero dimensions.
    /// </remarks>
    private static bool?[,] ConvertToMultidimensionalArray(List<List<bool?>> list)
    {
        if (list.Count == 0 || list[0].Count == 0)
            return new bool?[0, 0];

        int rows = list.Count;
        int cols = list[0].Count;
        var array = new bool?[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = list[i][j];
            }
        }
        return array;
    }
}
