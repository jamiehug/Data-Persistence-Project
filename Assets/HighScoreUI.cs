using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    public TextMeshProUGUI HighScoresText;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.GameData != null && GameManager.Instance.GameData.HighScores != null &&
            GameManager.Instance.GameData.HighScores.Count > 0)
        {
            StringBuilder sb = new();
            int index = 1;
            foreach (var highScore in GameManager.Instance.GameData.HighScores)
            {
                sb.AppendLine($"{index++}. {highScore.Name}   -   {highScore.Name}");
            }

            HighScoresText.text = sb.ToString();
            HighScoresText.fontSize = 85 - (index *2);
        }
        else
        {
            HighScoresText.text = "No high scores yet! Play to add your name to the high scores.";
            HighScoresText.fontSize = 45;
        }
    }   
}
