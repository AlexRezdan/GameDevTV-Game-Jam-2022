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

    private string currentAnimation;
    private const string IDLE = "Idle";
    private const string COLLECT = "Collect";
    private const string INACTIVE = "Inactive";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            StartCoroutine(Collect());
            switch (spriteType)
            {
                case SpriteType.Girl:
                    Player.Instance.nextSpriteType = CurrentSprite.Girl;
                    break;
                case SpriteType.Cat:
                    Player.Instance.nextSpriteType = CurrentSprite.Cat;
                    break;
                case SpriteType.Bird:
                    Player.Instance.nextSpriteType = CurrentSprite.Bird;
                    break;
                case SpriteType.Fish:
                    Player.Instance.nextSpriteType = CurrentSprite.Fish;
                    break;
                default: // Defaults to Girl if something goes wrong.
                    Player.Instance.nextSpriteType = CurrentSprite.Girl;
                    break;
            }
        }
    }

    private IEnumerator Collect()
    {
        ChangeAnimationState(COLLECT);
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
