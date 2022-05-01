using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(1000)]
public class MenuUI : MonoBehaviour
{

    public TMP_InputField PlayerNameCtrl;
    public TextMeshProUGUI HighScoreText;
    
    // Start is called before the first frame update
    void Start()
    {
        var topScore = GameManager.Instance.GetHighScorer();
        if (topScore != null)
            HighScoreText.text = $"High Score {topScore.Score} by {topScore.Name}";
        else
            HighScoreText.text = string.Empty;

        if (string.IsNullOrEmpty(GameManager.Instance.GameData.LastPlayerName) == false)
            PlayerNameCtrl.text = GameManager.Instance.GameData.LastPlayerName;

    }

    /// <summary>
    /// Starts a new game
    /// </summary>
    public void StartNew()
    {        
        // Why is TextMeshProUGUI.text always at least 1 character, even when no input given by
        // the user?
        var name = PlayerNameCtrl.text.Trim();        
        if (name.Length > 1)
        {
            // Initialize the current player's name
            GameManager.Instance.InitializeCurrentPlayer(PlayerNameCtrl.text);
            SceneManager.LoadScene(1);
        }
    }

    /// <summary>
    /// Quits the game, if playing within editor that call the editor application object
    /// to exit play mode
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        GameManager.Instance.SaveGameData();
    }
}
