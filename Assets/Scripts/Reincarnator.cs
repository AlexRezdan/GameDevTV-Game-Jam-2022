using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Animator), typeof(SpriteRenderer))]
public class Reincarnator : MonoBehaviour
{
    enum SpriteType { Girl, Cat, Bird, Fish }

    [SerializeField] private SpriteType spriteType;

    [SerializeField] private Sprite girlCoinSprite;
    [SerializeField] private Sprite catCoinSprite;
    [SerializeField] private Sprite birdCoinSprite;
    [SerializeField] private Sprite fishCoinSprite;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    [SerializeField] private AudioClip collectSound;

    private string currentAnimation;
    private const string IDLE = "Idle";
    private const string COLLECT = "Collect";
    private const string INACTIVE = "Inactive";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Ensures that a coin is not accidentally given the wrong coin sprite than what is assigned.
        AssignCorrectSprite();
    }

    private void AssignCorrectSprite()
    {
        switch (spriteType)
        {
            case SpriteType.Girl:
                spriteRenderer.sprite = girlCoinSprite;
                break;
            case SpriteType.Cat:
                spriteRenderer.sprite = catCoinSprite;
                break;
            case SpriteType.Bird:
                spriteRenderer.sprite = birdCoinSprite;
                break;
            case SpriteType.Fish:
                spriteRenderer.sprite = fishCoinSprite;
                break;
            default: // Defaults to Girl if something goes wrong.
                spriteRenderer.sprite = girlCoinSprite;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.CanReincarnateNow();

            StartCoroutine(Collect());

            switch (spriteType)
            {
                // If statement prevents the toggle being overridden so transformation is always possible.
                case SpriteType.Girl:
                    if (Player.Instance.currentSprite == CurrentSprite.Girl) { return; }
                    else
                    {
                        Player.Instance.nextSpriteType = CurrentSprite.Girl;
                        UI.Instance.UpdateUI(Player.Instance.nextSpriteType);
                    }
                    break;
                case SpriteType.Cat:
                    if (Player.Instance.currentSprite == CurrentSprite.Cat) { return; }
                    else
                    {
                        Player.Instance.nextSpriteType = CurrentSprite.Cat;
                        UI.Instance.UpdateUI(Player.Instance.nextSpriteType);
                    }
                    break;
                case SpriteType.Bird:
                    if (Player.Instance.currentSprite == CurrentSprite.Bird) { return; }
                    else
                    {
                        Player.Instance.nextSpriteType = CurrentSprite.Bird;
                        UI.Instance.UpdateUI(Player.Instance.nextSpriteType);
                    }
                    break;
                case SpriteType.Fish:
                    if (Player.Instance.currentSprite == CurrentSprite.Fish) { return; }
                    else
                    {
                        Player.Instance.nextSpriteType = CurrentSprite.Fish;
                        UI.Instance.UpdateUI(Player.Instance.nextSpriteType);
                    }
                    break;
                default: // Defaults to Girl if something goes wrong.
                    if (Player.Instance.currentSprite == CurrentSprite.Girl) { return; }
                    else
                    {
                        Player.Instance.nextSpriteType = CurrentSprite.Girl;
                        UI.Instance.UpdateUI(Player.Instance.nextSpriteType);
                    }
                    break;
            }
        }
    }

    private IEnumerator Collect()
    {
        ChangeAnimationState(COLLECT);
        audioSource.PlayOneShot(collectSound);
        yield return new WaitForSeconds(1f);
        ChangeAnimationState(IDLE);
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
