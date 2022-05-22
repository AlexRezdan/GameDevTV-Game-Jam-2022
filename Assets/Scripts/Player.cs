using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteAnimator
{
    public Animator animator;
    public AnimationClip[] animations;
}

public class Player : MonoBehaviour
{
    enum CurrentSprite { Girl, Cat, Bird, Fish }

    [Header("Sprite References")]
    // Allows you to set the currently controlled character to start the level as any character.
    [SerializeField] private CurrentSprite currentSprite;
    public int currentSpriteAnimations;
    //[SerializeField] private GameObject girlSprite;
    //[SerializeField] private GameObject catSprite;
    //[SerializeField] private GameObject birdSprite;

    [Header("Control Preferences")]
    private float horizontalInput;
    [Tooltip("How quickly the player moves left and right.")]
    [SerializeField] private float moveSpeed = 15.0f;
    [Tooltip("How high the player can jump.")]
    [SerializeField] private float jumpForce = 15.0f;
    [Tooltip("How quickly the player falls from maximum jump height (to avoid floaty jump).")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [Tooltip("How quickly the player falls when jump button is let go before maximum jump height.")]
    [SerializeField] private float lowJumpMultiplier = 2.0f;

    [SerializeField] private SpriteAnimator[] spriteAnimator;

    private bool isGrounded;

    private Rigidbody2D rb2d;

    private string currentAnimation;
    private const string IDLE = "Idle";
    private const string WALK = "Walk";
    private const string JUMP = "Jump";
    private const string FLY = "Fly";

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (currentSprite == CurrentSprite.Girl || currentSprite == CurrentSprite.Cat)
        {
            Jump();
        }
        else if (currentSprite == CurrentSprite.Bird)
        {
            Fly();
        }
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (currentSprite == CurrentSprite.Bird && isGrounded)
        {
            transform.Translate(Vector2.right * horizontalInput * (moveSpeed / 3) * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);
        }

        if (horizontalInput > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (isGrounded)
        {
            if (horizontalInput != 0)
            {
                ChangeAnimationState(WALK);
            }
            else
            {
                ChangeAnimationState(IDLE);
            }
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2d.velocity = Vector2.up * jumpForce;
        }

        if (!isGrounded)
        {
            ChangeAnimationState(JUMP);
        }

        if (rb2d.velocity.y < 0)  // If the player is falling
        {
            rb2d.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))  // If the player lets go of the Jump button during the jump
        {
            rb2d.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Fly()
    {
        if (Input.GetButton("Jump"))
        {
            rb2d.velocity = Vector2.up * jumpForce;
            ChangeAnimationState(FLY);
        }
        else
        {
            rb2d.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            ChangeAnimationState(IDLE);
        }
    } 

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    
    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        SetCurrentCharacterAnimator(currentSpriteAnimations);

        // currentSpriteAnimations comes from the Reincarnator script so the correct animation plays.
        spriteAnimator[currentSpriteAnimations].animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    private void SetCurrentCharacterAnimator(int currentSpriteAnimations)
    {
        switch (currentSpriteAnimations)
        {
            case 0:
                currentSprite = CurrentSprite.Girl;
                ChangeColliderSize(CurrentSprite.Girl);
                break;
            case 1:
                currentSprite = CurrentSprite.Cat;
                ChangeColliderSize(CurrentSprite.Cat);
                break;
            case 2:
                currentSprite = CurrentSprite.Bird;
                ChangeColliderSize(CurrentSprite.Bird);
                break;
            default:
                Debug.LogError("Current Sprite integer not found!");
                break;
        }
    }

    private void ChangeColliderSize(CurrentSprite currentSprite)
    {
        CapsuleCollider2D capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        switch (currentSprite)
        {
            case CurrentSprite.Girl:
                capsuleCollider2D.offset = new Vector2(0.3023667f, -0.2159758f);
                capsuleCollider2D.size = new Vector2(2.19901f, 4.099597f);
                capsuleCollider2D.direction = CapsuleDirection2D.Vertical;
                break;
            case CurrentSprite.Cat:
                capsuleCollider2D.offset = new Vector2(0.3408749f, -1.293232f);
                capsuleCollider2D.size = new Vector2(1.367718f, 1.945085f);
                capsuleCollider2D.direction = CapsuleDirection2D.Vertical;
                break;
            case CurrentSprite.Bird:
                capsuleCollider2D.offset = new Vector2(0.7973438f, -0.07597946f);
                capsuleCollider2D.size = new Vector2(4.836072f, 2.614592f);
                capsuleCollider2D.direction = CapsuleDirection2D.Horizontal;
                break;
        }
    }
}
