using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reincarnator : MonoBehaviour
{
    /* Indexes follow in this order
     * Girl: 0
     * Cat: 1
     * Bird: 2
     * Fish: 3
    */
    enum SpriteType { Girl, Cat, Bird, Fish }

    [SerializeField] private SpriteType spriteType;

    public GameObject girlSprite;
    public GameObject catSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (spriteType == SpriteType.Girl)
            {
                catSprite.SetActive(false);
                girlSprite.SetActive(true);
                player.currentSpriteAnimations = 0;
            }
            if (spriteType == SpriteType.Cat)
            {
                girlSprite.SetActive(false);
                catSprite.SetActive(true);
                player.currentSpriteAnimations = 1;
            }
        }
    }
}
