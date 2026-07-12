using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("UI")]
    public TMP_Text scoreText;

    [Header("Scoring")]
    public float scorePerUnit = 1f;

    float startX;
    public int currentScore;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (player != null)
        {
            startX = player.position.x;
        }

        UpdateScoreText();
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        float distanceX = Mathf.Max(0f, player.position.x - startX);
        currentScore = Mathf.FloorToInt(distanceX * scorePerUnit);

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}