using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reincarnator : MonoBehaviour
{
    enum SpriteType { Girl, Cat, Bird, Fish }

    [SerializeField] private SpriteType spriteType;

    public GameObject girlSprite;
    public GameObject catSprite;
    public GameObject birdSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (spriteType == SpriteType.Girl)
            {
                catSprite.SetActive(false);
                birdSprite.SetActive(false);
                girlSprite.SetActive(true);
                player.currentSpriteAnimations = 0;
            }
            if (spriteType == SpriteType.Cat)
            {
                girlSprite.SetActive(false);
                birdSprite.SetActive(false);
                catSprite.SetActive(true);
                player.currentSpriteAnimations = 1;
            }
            if (spriteType == SpriteType.Bird)
            {
                girlSprite.SetActive(false);
                catSprite.SetActive(false);
                birdSprite.SetActive(true);
                player.currentSpriteAnimations = 2;
            }
        }
    }
}
