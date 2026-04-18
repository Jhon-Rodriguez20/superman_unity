using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private int currentScore = 0;  // Puntuación actual
    private int highScore = 0;      // Mejor récord

    void Start()
    {
        // Cargar el mejor récord guardado
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();

        // Inicializar puntuación
        currentScore = 0;
        UpdateScoreUI();

    }

    // Llamar este método cuando Superman esquive una nube
    public void AddScore(int points = 10)
    {
        currentScore += points;
        UpdateScoreUI();

        // Verificar si es nuevo récord
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            UpdateHighScoreUI();
        }
    }

    // Actualizar el texto de puntuación actual
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
    }

    // Actualizar el texto del mejor récord
    void UpdateHighScoreUI()
    {
        if (highScoreText != null)
            highScoreText.text = "H.I. Score: " + highScore;
    }

    // Obtener la puntuación actual (para otros scripts)
    public int GetCurrentScore()
    {
        return currentScore;
    }

    // Reiniciar la puntuación (cuando se reinicia el juego)
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }
}