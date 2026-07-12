using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class DeathUI : MonoBehaviour
{
    public TMP_Text scoreManager;
    public TMP_Text scoreText;

    public void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowScore()
    {
        if (scoreManager != null)
        {
            string finalScore = scoreManager.text;
            if (scoreText != null)
            {
                scoreText.text = "You ran " + finalScore + "m before hitting a wall and tumbling to your death!";
            }
        }
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame|| Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    
}