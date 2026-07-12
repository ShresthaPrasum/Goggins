using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	public float startXSpeed = 4f;
	public float maxXSpeed = 12f;
	public float xAcceleration = 6f;
	public float jumpForce = 12f;
	public float gravityScale = 4f;
	public Transform groundCheck;
	public LayerMask groundLayer;
	public float groundCheckRadius = 0.15f;

	public Animator animator;

	Rigidbody2D rb;
	Collider2D playerCollider;
	PhysicsMaterial2D noFrictionMaterial;
	public float currentXSpeed;

	public GameObject deathScreen;
	bool jumpQueued;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();
	}

	void Start()
	{
		currentXSpeed = startXSpeed;
		rb.gravityScale = gravityScale;

		rb.freezeRotation = true;

		rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;



		if (playerCollider != null)
		{
			noFrictionMaterial = new PhysicsMaterial2D("PlayerNoFriction");

			noFrictionMaterial.friction = 0f;

			noFrictionMaterial.bounciness = 0f;

			playerCollider.sharedMaterial = noFrictionMaterial;
		}
	}

	void Update()

	{
		Keyboard keyboard = Keyboard.current;

		if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
		{
			jumpQueued = true;
			Debug.Log("Player just pressed the space key");

		}
		else if (keyboard == null && Input.GetKeyDown(KeyCode.Space))
		{
			jumpQueued = true;
			Debug.Log("Player just pressed the space key");
		}
	}

	void FixedUpdate()

	{
		bool grounded = IsGrounded();

		
		currentXSpeed = Mathf.MoveTowards(currentXSpeed, maxXSpeed, xAcceleration * Time.fixedDeltaTime);

		Vector2 velocity = rb.linearVelocity;
		velocity.x = currentXSpeed;

		
		if (jumpQueued && grounded)
		{

			velocity.y =  jumpForce;
			jumpQueued = false;
		}

		
		if (grounded && velocity.y < 0f)
		{
			    velocity.y = 0f;
		}

		
		if (Mathf.Abs(rb.linearVelocity.x) < 0.01f && currentXSpeed > 0f)
		{
			    velocity.x = Mathf.Min(velocity.x, rb.linearVelocity.x + 0.1f);
		}

		rb.linearVelocity = velocity;

		animator.SetBool("Jump", !grounded);
		if (grounded)
		{

			jumpQueued = false;
		}
	}

	bool IsGrounded()
	{
		if (groundCheck == null)

		{
			return false;

		}

		return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
	}

	public void Die()
	{
		if (deathScreen != null)
        {
            deathScreen.SetActive(true);		
			DeathUI deathUi = deathScreen.GetComponentInChildren<DeathUI>(true);
			if (deathUi != null)
			{
				deathUi.ShowScore();
			}
        }
	}
}
