using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private Rigidbody2D obstacleRb;
    private Collider2D obstacleCollider;
    private SpriteRenderer[] spriteRenderers;
    public float destroyDelay = 2f;
    private bool destroyTimerStarted;

    void Start()
    {

        obstacleRb = GetComponent<Rigidbody2D>();
        obstacleCollider = GetComponent<Collider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (playerScript != null && playerScript.currentXSpeed > 7f) 
            {
                playerScript.currentXSpeed -= 1f; 
            }
            
            if (obstacleRb != null)
            {
                obstacleRb.linearVelocity = new Vector2(0f, 5f);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            obstacleCollider.enabled = false;
            obstacleRb.gravityScale = 0.4f;

            if (!destroyTimerStarted)
            {
                destroyTimerStarted = true;
                StartCoroutine(FadeOutAndDestroy());
            }
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsed = 0f;

        while (elapsed < destroyDelay)
        {
            float progress = elapsed / destroyDelay;
            float alpha = Mathf.Lerp(1f, 0f, progress);
            float blink = Mathf.PingPong(Time.time * 12f, 1f);

            if (spriteRenderers != null)
            {
                for (int i = 0; i < spriteRenderers.Length; i++)
                {
                    SpriteRenderer spriteRenderer = spriteRenderers[i];
                    if (spriteRenderer != null)
                    {
                        Color color = spriteRenderer.color;
                        color.a = alpha * blink;
                        spriteRenderer.color = color;
                    }
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}