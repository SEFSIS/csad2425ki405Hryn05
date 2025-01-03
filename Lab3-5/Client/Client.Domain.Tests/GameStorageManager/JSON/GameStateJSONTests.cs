using Client.Domain.Services.GameServices;
using Client.Domain.Services.GameStorageManager.JSON;
using Client.Domain.Services.Settings.GameSettingsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Domain.Tests.GameStorageManager.JSON;

[TestClass]
public class GameStateJSONTests
{
    [TestMethod]
    public void Constructor_ShouldInitializeEmptyBoard()
    {
        var gameStateJSON = new GameStateJSON();

        Assert.IsNotNull(gameStateJSON.Board);
        Assert.AreEqual(0, gameStateJSON.Board.Count);
    }

    [TestMethod]
    public void Constructor_ShouldConvertGameStateToGameStateJSON()
    {
        var gameState = new GameState
        {
            Board = new bool?[,]
            {
                    { true, false, null },
                    { null, true, false }
            },
            Mode = GameMode.ManvsMan,
            Status = GameStatus.Ongoing,
            ManPlayer = true
        };

        var gameStateXML = new GameStateJSON(gameState);

        Assert.AreEqual(GameMode.ManvsMan, gameStateXML.Mode);
        Assert.AreEqual(GameStatus.Ongoing, gameStateXML.Status);
        Assert.AreEqual(true, gameStateXML.ManPlayer);
        Assert.AreEqual(2, gameStateXML.Board.Count);
        Assert.AreEqual(3, gameStateXML.Board[0].Count);
        Assert.AreEqual(true, gameStateXML.Board[0][0]);
        Assert.AreEqual(null, gameStateXML.Board[1][0]);
    }

    [TestMethod]
    public void ToGameState_ShouldConvertGameStateJSONToGameState()
    {
        var nestedList = new List<List<bool?>>
            {
                new List<bool?> { false, false, null },
                new List<bool?> { true, false, null },
                new List<bool?> { null, true, false }
            };

        var gameStateXML = new GameStateJSON
        {
            Board = nestedList,
            Mode = GameMode.ManvsAI,
            Status = GameStatus.WonPlayerO,
            ManPlayer = false
        };

        var gameState = gameStateXML.ToGameState();

        Assert.AreEqual(GameMode.ManvsAI, gameState.Mode);
        Assert.AreEqual(GameStatus.WonPlayerO, gameState.Status);
        Assert.AreEqual(false, gameState.ManPlayer);
        Assert.AreEqual(nestedList.Count, gameState.Board.GetLength(0));
        Assert.AreEqual(3, gameState.Board.GetLength(1));
        Assert.AreEqual(false, gameState.Board[0, 0]);
        Assert.AreEqual(true, gameState.Board[1, 0]);
    }
}
