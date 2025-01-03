using Client.Domain.Services.GameServices;
using Client.Domain.Services.IStorageManager;
using System.IO;
using System.Text.Json;

namespace Client.Domain.Services.GameStorageManager.JSON;

/// <summary>
/// Implementation of <see cref="IGameStorageManager"/> that uses JSON serialization for saving and loading game states.
/// </summary>
public class GameStorageJSONManager : IGameStorageManager
{
    /// <summary>
    /// Default folder path for saving and loading game files.
    /// </summary>
    private readonly string _defaultFolder = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\Games"));

    /// <summary>
    /// Loads a game state from an JSON file.
    /// </summary>
    /// <returns>The loaded <see cref="GameState"/> object.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the JSON file cannot be deserialized into a <see cref="GameStateJSON"/>.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the file specified by the user does not exist.</exception>
    public GameState LoadGame()
    {
        GameStateJSON readedState = new();
        string path = GetPath(_defaultFolder, false);
        if (string.IsNullOrEmpty(path))
            return new GameState();

        using (StreamReader reader = new StreamReader(path))
        {
            string jsonContent = reader.ReadToEnd();
            readedState = JsonSerializer.Deserialize<GameStateJSON>(jsonContent);
        }

        return readedState.ToGameState();
    }

    /// <summary>
    /// Saves a game state to an JSON file.
    /// </summary>
    /// <param name="game">The game state to save.</param>
    /// <exception cref="InvalidOperationException">Thrown if the <see cref="GameState"/> cannot be serialized.</exception>
    public void SaveGame(GameState game)
    {
        GameStateJSON state = new(game);
        string path = GetPath(_defaultFolder);

        if (string.IsNullOrEmpty(path))
            return;

        using (StreamWriter writer = new StreamWriter(path))
        {
            string jsonContent = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
            writer.Write(jsonContent);
        }
    }

    /// <summary>
    /// Retrieves the file path for saving or loading JSON files.
    /// </summary>
    /// <param name="defaultPath">The default folder path to use.</param>
    /// <param name="saveFile"><c>true</c> to get a save path, <c>false</c> to get a load path.</param>
    /// <returns>The file path as a string.</returns>
    private string GetPath(string defaultPath = "", bool saveFile = true)
    {
        string path = string.IsNullOrEmpty(defaultPath) ? "C:\\" : defaultPath;

        return saveFile ? GetPathToSaveJson(path) : GetPathToLoadJson(path);
    }

    /// <summary>
    /// Opens a dialog for selecting a file to save as JSON.
    /// </summary>
    /// <param name="defaultPath">The default folder path for the dialog.</param>
    /// <returns>The selected file path, or <c>null</c> if the dialog is canceled.</returns>
    public static string GetPathToSaveJson(string defaultPath)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save JSON File",
            Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
            DefaultExt = "json",
            InitialDirectory = defaultPath
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            return saveFileDialog.FileName;
        }

        return null;
    }

    /// <summary>
    /// Opens a dialog for selecting an JSON file to load.
    /// </summary>
    /// <param name="defaultPath">The default folder path for the dialog.</param>
    /// <returns>The selected file path, or <c>null</c> if the dialog is canceled.</returns>
    public static string GetPathToLoadJson(string defaultPath)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Load JSON File",
            Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
            InitialDirectory = defaultPath
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            return openFileDialog.FileName;
        }

        return null;
    }
}
