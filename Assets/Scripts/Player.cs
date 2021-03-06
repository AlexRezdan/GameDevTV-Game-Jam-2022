using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteAnimator
{
    public Animator animator;
    // public AnimationClip[] animations; **OLD WAY OF ASSIGNING MULTIPLE ANIMATIONS PER ANIMATOR**
}

public enum CurrentSprite { Girl, Cat, Bird, Fish, Zombie }

[RequireComponent(typeof(CapsuleCollider2D), typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Tooltip("Should the player start this level as a zombie?")]
    [SerializeField] private bool startLevelAsZombie = true;

    [Header("Sprite References")]
    [Tooltip("Make sure to assign correct Current Sprite Animations!")]
    public CurrentSprite currentSprite;
    [Tooltip("See Sprite Animator below for correct number.")]
    public int currentSpriteAnimations;

    public CurrentSprite nextSpriteType;

    [Header("Girl Parameters")]
    public GameObject girlSprite;
    [Tooltip("How quickly the player moves left and right.")]
    [SerializeField] private float moveSpeed = 15.0f;
    [Tooltip("How high the player can jump.")]
    [SerializeField] private float jumpForce = 15.0f;
    [Tooltip("How quickly the player falls from maximum jump height (to avoid floaty jump).")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [Tooltip("How quickly the player falls when jump button is let go before maximum jump height.")]
    [SerializeField] private float lowJumpMultiplier = 2.0f;

    [Header("Cat Parameters")]
    public GameObject catSprite;

    [Header("Bird Parameters")]
    public GameObject birdSprite;

    [Header("Fish Parameters")]
    public GameObject fishSprite;

    [Header("Zombie Parameters")]
    public GameObject zombieSprite;

    private float horizontalInput;

    private bool isGrounded;
    private bool isDead;
    public bool nextSpriteExists;

    private Rigidbody2D rb2d;

    private CapsuleCollider2D capsuleCollider2D;

    [SerializeField] private SpriteAnimator[] spriteAnimator;

    [SerializeField] private ParticleSystem reincarnateParticleEffect;

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepSounds;

    private string currentAnimation;
    private const string IDLE = "Idle";
    private const string WALK = "Walk";
    private const string JUMP = "Jump";
    private const string FLY = "Fly";
    private const string DEAD = "Dead";

    // Singleton Instantion
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<Player>();
            return instance;
        }
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        if (startLevelAsZombie)
        {
            StartCoroutine(WaitForZombie());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (!DialogueManagerInk.GetInstance().dialogueIsPlaying)
            {
                Move();
                Reincarnate();

                if (currentSprite == CurrentSprite.Girl || currentSprite == CurrentSprite.Cat)
                {
                    Jump();
                }
                else if (currentSprite == CurrentSprite.Bird)
                {
                    Fly();
                }
                else if (currentSprite == CurrentSprite.Fish)
                {
                    Swim();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private IEnumerator WaitForZombie()
    {
        isDead = true;
        StartLevelAsZombie();
        yield return new WaitForSeconds(1f); // Wait for zombie "Appear" animation to finish before being able to move.
        isDead = false;
    }

    private void StartLevelAsZombie()
    {
        girlSprite.SetActive(false);
        catSprite.SetActive(false);
        birdSprite.SetActive(false);
        fishSprite.SetActive(false);
        zombieSprite.SetActive(true);
        nextSpriteExists = false;
        currentSprite = CurrentSprite.Zombie;
        currentSpriteAnimations = 4;
        ChangeColliderSize(CurrentSprite.Girl);
    }

    public void CanReincarnateNow()
    {
        if (nextSpriteExists == true) { return; }
        else
        {
            nextSpriteExists = true;
        }
    }

    private void Reincarnate()
    {
        if (Input.GetKeyDown(KeyCode.R) && nextSpriteExists)
        {
            StartCoroutine(DieAndWait());   
        }
    }

    private IEnumerator DieAndWait()
    {
        CurrentSprite toggleSprite;
        if (currentSprite == CurrentSprite.Zombie)
        {
            toggleSprite = nextSpriteType;  // Zombie can only be starter sprite, so first token collected won't be toggleable yet.
        }
        else
        {
            toggleSprite = currentSprite;
        }

        isDead = true;
        ChangeAnimationState(DEAD);
        yield return new WaitForSeconds(2f);
        ChangeAnimationState(IDLE);

        switch (nextSpriteType)
        {
            case CurrentSprite.Girl:
                zombieSprite.SetActive(false);
                catSprite.SetActive(false);
                birdSprite.SetActive(false);
                fishSprite.SetActive(false);
                girlSprite.SetActive(true);
                currentSpriteAnimations = 0;
                nextSpriteType = toggleSprite;
                currentSprite = CurrentSprite.Girl;
                UI.Instance.UpdateUI(nextSpriteType);
                break;
            case CurrentSprite.Cat:
                zombieSprite.SetActive(false);
                girlSprite.SetActive(false);
                birdSprite.SetActive(false);
                fishSprite.SetActive(false);
                catSprite.SetActive(true);
                currentSpriteAnimations = 1;
                nextSpriteType = toggleSprite;
                currentSprite = CurrentSprite.Cat;
                UI.Instance.UpdateUI(nextSpriteType);
                break;
            case CurrentSprite.Bird:
                zombieSprite.SetActive(false);
                girlSprite.SetActive(false);
                catSprite.SetActive(false);
                fishSprite.SetActive(false);
                birdSprite.SetActive(true);
                currentSpriteAnimations = 2;
                nextSpriteType = toggleSprite;
                currentSprite = CurrentSprite.Bird;
                UI.Instance.UpdateUI(nextSpriteType);
                break;
            case CurrentSprite.Fish:
                zombieSprite.SetActive(false);
                girlSprite.SetActive(false);
                catSprite.SetActive(false);
                birdSprite.SetActive(false);
                fishSprite.SetActive(true);
                currentSpriteAnimations = 3;
                nextSpriteType = toggleSprite;
                currentSprite = CurrentSprite.Fish;
                UI.Instance.UpdateUI(nextSpriteType);
                break;
            default: // Defaults to Girl if something goes wrong.
                zombieSprite.SetActive(false);
                catSprite.SetActive(false);
                birdSprite.SetActive(false);
                fishSprite.SetActive(false);
                girlSprite.SetActive(true);
                currentSpriteAnimations = 0;
                nextSpriteType = toggleSprite;
                currentSprite = CurrentSprite.Girl;
                UI.Instance.UpdateUI(nextSpriteType);
                break;
        }

        StartCoroutine(PlayParticleEffectOnce());

        isDead = false;
    }

    private IEnumerator PlayParticleEffectOnce()
    {
        reincarnateParticleEffect.Play();
        yield return new WaitForSeconds(1f);
        reincarnateParticleEffect.Stop();
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if ((currentSprite == CurrentSprite.Bird && isGrounded) || (currentSprite == CurrentSprite.Zombie && isGrounded))
        {
            transform.Translate(Vector2.right * horizontalInput * (moveSpeed / 4) * Time.deltaTime);
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
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
                }
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

    private void Swim()
    {
        if (Input.GetButton("Jump"))
        {
            rb2d.velocity = Vector2.up * jumpForce;
            ChangeAnimationState(IDLE);
        }
        else
        {
            rb2d.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            ChangeAnimationState(IDLE);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (currentSprite == CurrentSprite.Girl || currentSprite == CurrentSprite.Bird || currentSprite == CurrentSprite.Zombie)
        {
            if (collision.gameObject.CompareTag("GroundChecker"))
            {
                isGrounded = true;
            }
        }
        if (currentSprite == CurrentSprite.Cat)
        {
            if (collision.gameObject.CompareTag("GroundChecker") || collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (currentSprite == CurrentSprite.Girl || currentSprite == CurrentSprite.Bird || currentSprite == CurrentSprite.Zombie)
        {
            if (collision.gameObject.CompareTag("GroundChecker"))
            {
                isGrounded = false;
            }
        }
        if (currentSprite == CurrentSprite.Cat)
        {
            if (collision.gameObject.CompareTag("GroundChecker") || collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
    }
    
    public void ChangeAnimationState(string newAnimation)
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
            case 3:
                currentSprite = CurrentSprite.Fish;
                ChangeColliderSize(CurrentSprite.Fish);
                break;
            case 4:
                currentSprite = CurrentSprite.Zombie;
                ChangeColliderSize(CurrentSprite.Girl);
                break;
            default:
                Debug.LogError("Current Sprite integer not found!");
                break;
        }
    }

    private void ChangeColliderSize(CurrentSprite currentSprite)
    {
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
            case CurrentSprite.Fish:
                capsuleCollider2D.offset = new Vector2(-0.02556229f, 0.1634107f);
                capsuleCollider2D.size = new Vector2(1.095589f, 1.783848f);
                capsuleCollider2D.direction = CapsuleDirection2D.Vertical;
                break;
        }
    }
}
