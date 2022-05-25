using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Sprite girlSprite;
    public Sprite catSprite;
    public Sprite birdSprite;
    public Sprite fishSprite;

    private Image image;

    [SerializeField] TextMeshProUGUI pressHText;
    [SerializeField] GameObject helpText;

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
        image = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (pressHText.enabled)
            {
                pressHText.enabled = false;
                helpText.SetActive(true);
            }
            else if (helpText.activeInHierarchy)
            {
                helpText.SetActive(false);
                pressHText.enabled = true;
            }
        }
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
