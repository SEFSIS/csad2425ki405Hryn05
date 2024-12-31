using Client.Domain.Services.GameService;
using Client.Domain.Services.IStorageManager;
using System.IO;
using System.Text.Json;

namespace Client.Domain.Services.GameStorageManager.JSON;

public class GameStorageJSONManager : IGameStorageManager
{
    private readonly string _defaultFolder = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Client\Games"));

    public GameState LoadGame()
    {
        GameStateJSON readedState = new();
        string path = GetPath(_defaultFolder, false);

        using (StreamReader reader = new StreamReader(path))
        {
            string jsonContent = reader.ReadToEnd();
            readedState = JsonSerializer.Deserialize<GameStateJSON>(jsonContent);
        }

        return readedState.ToGameState();
    }

    public void SaveGame(GameState game)
    {
        GameStateJSON state = new(game);
        string path = GetPath(_defaultFolder);

        using (StreamWriter writer = new StreamWriter(path))
        {
            string jsonContent = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
            writer.Write(jsonContent);
        }
    }

    private string GetPath(string defaultPath = "", bool saveFile = true)
    {
        string path = string.IsNullOrEmpty(defaultPath) ? "C:\\" : defaultPath;

        return saveFile ? GetPathToSaveJson(path) : GetPathToLoadJson(path);
    }

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
