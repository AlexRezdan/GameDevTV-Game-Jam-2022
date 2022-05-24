using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Sprite girlSprite;
    public Sprite catSprite;
    public Sprite birdSprite;
    public Sprite fishSprite;

    private Image image;

    // Singleton Instantion
    private static UI instance;
    public static UI Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<UI>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    public void UpdateUI(CurrentSprite currentSprite)
    {
        switch (currentSprite)
        {
            case CurrentSprite.Girl:
                image.sprite = girlSprite;
                break;
            case CurrentSprite.Cat:
                image.sprite = catSprite;
                break;
            case CurrentSprite.Bird:
                image.sprite = birdSprite;
                break;
            case CurrentSprite.Fish:
                image.sprite = fishSprite;
                break;
            default:
                image.sprite = girlSprite;
                break;
        }
    }
}
