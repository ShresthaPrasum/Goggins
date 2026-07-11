using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D obstacleRb;
    private Collider2D obstacleCollider;

    void Start()
    {
        animator = GetComponent<Animator>(); 

        obstacleRb = GetComponent<Rigidbody2D>();
        obstacleCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (animator != null)
            {
                animator.SetBool("hasTriggered", true);
            }
            
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.currentXSpeed -= 2f; 
            }
            
            if (obstacleRb != null)
            {
                obstacleRb.linearVelocity = new Vector2(0f, 5f);
            }
         
            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
            }
        }
    }
}