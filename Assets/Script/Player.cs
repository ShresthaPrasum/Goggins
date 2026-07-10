using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float gravity;
    public float maxGravityMultiplier = 2.2f;
    public Vector2 velocity;
    public float maxXVelocity = 100;
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float distance = 0;
    public float jumpVelocity = 20;
    public bool isGrounded = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.15f;

    bool jumpRequested;

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.spaceKey.wasPressedThisFrame)
            {
                jumpRequested = true;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        bool groundedNow = IsGrounded();

        if (jumpRequested && groundedNow)
        {
            velocity.y = jumpVelocity;
            groundedNow = false;
            isGrounded = false;
            jumpRequested = false;
        }
        else if (groundedNow)
        {
            velocity.y = 0f;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (!groundedNow)
        {
            float speedRatio = Mathf.Clamp01(velocity.x / Mathf.Max(0.01f, maxXVelocity));
            float gravityMultiplier = Mathf.Lerp(1f, maxGravityMultiplier, speedRatio);
            float effectiveGravity = gravity * gravityMultiplier;

            pos.y += velocity.y * Time.fixedDeltaTime;
            velocity.y += effectiveGravity * Time.fixedDeltaTime;
        }

        distance += velocity.x * Time.fixedDeltaTime;

        pos.x += velocity.x * Time.fixedDeltaTime;

        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - Mathf.Clamp01(velocityRatio));

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }
        }

        jumpRequested = false;

        transform.position = pos;
    }

    bool IsGrounded()
    {
        if (groundCheck != null)
        {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
        }

        // Fallback for old scenes if groundCheck is not assigned yet.
        return transform.position.y <= 0f;
    }

}
