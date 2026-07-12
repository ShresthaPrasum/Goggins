using UnityEngine;
using TMPro;


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
    
}