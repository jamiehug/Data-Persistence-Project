using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int MaxHighScores = 10;

    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Store up-to 10 high scores
    /// </summary>
    public GameSaveData GameData { get; set; } = new GameSaveData();

    private void Awake()
    {
        if (Instance == null)
        {
            _gameDataFilename = Path.Combine(Application.persistentDataPath, "gameData.json");

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LoadGameData();
    }

    #region Public Properties
    public PlayerInfo CurrentPlayer { get; private set; }
    #endregion

    #region Public Methods
    public void InitializeCurrentPlayer(string name)
    {
        CurrentPlayer = new PlayerInfo(name);
        GameData.LastPlayerName = name;
    }

    public void GameStart()
    {        
        // Calculate if the player makes it into the high score table
        CalculateHighScore();
        CurrentPlayer = new PlayerInfo(CurrentPlayer.Name);

    }

    public void UpdateCurrentPlayerScore(int points)
    {
        if (CurrentPlayer != null)
        {
            CurrentPlayer.Score += points;           
        }
    }

    public void CurrentPlayerGameOver()
    {
        CalculateHighScore(true);
        SaveGameData();
    }

    public PlayerInfo GetHighScorer()
    {
        if( GameData != null )
            return GameData.HighScores.FirstOrDefault();

        return null;
    }

    public void SaveGameData()
    {
        var data = JsonUtility.ToJson(GameData);
        File.WriteAllText(_gameDataFilename, data);
    }
    #endregion

    #region Private Methods
    private void LoadGameData()
    {
        if( File.Exists( _gameDataFilename ) )
        {
            GameData = JsonUtility.FromJson<GameSaveData>(File.ReadAllText(_gameDataFilename));
            if( GameData.HighScores == null )
                GameData.HighScores = new List<PlayerInfo>();   
        }
        else
        {
            GameData = new GameSaveData
            {
                HighScores = new List<PlayerInfo>()
            };
        }
    }
    
    private void CalculateHighScore(bool addCurrentPlayer = false)
    {        
        if (addCurrentPlayer)
            GameData.HighScores.Add(CurrentPlayer);            
        
        // Sort the high scores from highest to lowest (DESC) and only then take the maximum allowed high scores
        GameData.HighScores = GameData.HighScores.OrderByDescending(highScore => highScore.Score).Take(MaxHighScores).ToList();
    }
    #endregion

    private string _gameDataFilename;
}

[System.Serializable]
public class GameSaveData
{
    public string LastPlayerName;

    public List<PlayerInfo> HighScores;
}

[System.Serializable]
public class PlayerInfo
{
    public PlayerInfo()
    {

    }

    public PlayerInfo(string name)
    {
        Name    = name;
        Score   = 0;
    }

    public string Name;

    public int Score;
}
