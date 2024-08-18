using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public int score = 0, highScore = 0;
    public int combo = 1;

    public bool canAddScore = true;

    public TMP_Text scoreTMP, comboTMP, highscoreTMP;

    Canvas canvas;

    public Canvas menuCanvas;

    private void Start()
    {
        // Retrieve the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Set the high score text in the UI
        highscoreTMP.text = highScore.ToString();

        // Get the canvas component
        canvas = GetComponent<Canvas>();
    }

    public void EnableCanvas()
    {
        canvas.enabled = true;
    }

    public void DisableMenuCanvas()
    {
        menuCanvas.enabled = false;
    }

    public void AddScore(int sizeOfCollision)
    {
        if (canAddScore)
        {
            // Add to the score based on collision size and combo multiplier
            score += sizeOfCollision * 10 * combo;
            scoreTMP.text = score.ToString();

            // Update high score if current score exceeds it
            if (score > highScore)
            {
                UpdateHighScore();
            }
        }
    }

    public void UpdateCombo(int comboToAdd = 1)
    {
        combo += comboToAdd;
        comboTMP.text = combo.ToString();
    }

    public void ResetCombo()
    {
        combo = 1;
        comboTMP.text = combo.ToString();
    }

    void UpdateHighScore()
    {
        // Set high score to current score and update PlayerPrefs
        highScore = score;
        highscoreTMP.text = highScore.ToString();
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save(); // Ensure the high score is saved immediately
    }

    public void RestartGame()
    {
        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
