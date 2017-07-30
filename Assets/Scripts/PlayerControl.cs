using UnityEngine;
using System.Collections;
using FTRuntime;

public class PlayerControl : MonoBehaviour {
	[HideInInspector] public bool facingRight = false;		// For determining which way the player is currently facing.
	[HideInInspector] public bool jump = false;				// Condition for whether the player should jump.
    [HideInInspector] public bool hasPlug = false;          // Is the player holding a plug?

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;         // Amount of force added when the player jumps.
    public Transform groundCheck;           // A position marking where to check if the player is grounded.

	private bool grounded = false;			// Whether or not the player is grounded.
    [HideInInspector] public SwfClipController anim;
    private Rigidbody2D body;

    private bool isPlayingSpecialAnim {
        get {
            return anim.loopMode == SwfClipController.LoopModes.Once;
        }
    }

	void Awake() {
        anim = GetComponent<SwfClipController>();
        anim.OnStopPlayingEvent += (SwfClipController c) => {
            c.loopMode = SwfClipController.LoopModes.Loop;
            c.PlayIfNotAlreadyPlaying(hasPlug ? "player-idleplug" : "player-idle");
        };
        body = GetComponent<Rigidbody2D>();
	}


	void Update() {
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump") && grounded && !isPlayingSpecialAnim) jump = true;

        //ANIMATIONS!
        if (anim.loopMode == SwfClipController.LoopModes.Loop) { //is not playing a special animation
            if (body.velocity.y > 0) { //jumping
                anim.PlayIfNotAlreadyPlaying(hasPlug ? "player-jumpplug" : "player-jump");
                return;
            }
            if (body.velocity.y < 0) { //falling
                anim.PlayIfNotAlreadyPlaying(hasPlug ? "player-fallplug" : "player-fall");
                return;
            }
            if (body.velocity.x != 0) { //moving
                anim.PlayIfNotAlreadyPlaying(hasPlug ? "player-walkplug" : "player-walk");
            } else {
                anim.PlayIfNotAlreadyPlaying(hasPlug ? "player-idleplug" : "player-idle");
            }
        }
	}


	void FixedUpdate () {
		// Cache the horizontal input.
		float h = isPlayingSpecialAnim ? 0 : Input.GetAxis("Horizontal");

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * body.velocity.x < maxSpeed)
			// ... add a force to the player.
			body.AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(body.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxSpeed, body.velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (h > 0 && facingRight) Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (h < 0 && !facingRight) Flip();

        // If the player should jump...
        if (jump)
		{
			// Set the Jump animator trigger parameter.
			//TODO anim.SetTrigger("Jump");

			// Play a random jump audio clip.
			int i = Random.Range(0, jumpClips.Length);
			AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
	}

    void Flip() {
        facingRight = !facingRight;
        //flop scale
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
