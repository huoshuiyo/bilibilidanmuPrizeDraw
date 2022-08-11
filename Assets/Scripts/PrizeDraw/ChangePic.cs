using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
public class ChangePic : MonoBehaviour
{
    public Sprite[] sprites;
    public CanvasGroup canvasGroup;
    public Image image;
    int spritesNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup= GetComponent<CanvasGroup>();
        spritesNum = 0;
        image = transform.Find("ShowImage").GetComponent<Image>();
        image.sprite = sprites[spritesNum];
        transform.Find("BackButton").GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }
            );
        transform.Find("NextButton").GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                spritesNum++;
                if (spritesNum == sprites.Length)
                {
                    spritesNum = 0;
                }
                image.sprite = sprites[spritesNum];
            }
            );

    }
    public void OpenPicPanel() 
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
