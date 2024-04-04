using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatImage : MonoBehaviour
{
    public Image imageGirl;
    public GameManager gameManager;

    public void ZoomImage()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        gameManager.imageZoom.sprite = imageGirl.sprite;
        gameManager.imageZoom.preserveAspect = true;
        gameManager.imageZoom.gameObject.SetActive(true);
        gameManager.imageZoomGallery.sprite = imageGirl.sprite;
        gameManager.imageZoomGallery.preserveAspect = true;
        gameManager.imageZoomGallery.gameObject.SetActive(true);
    }
}
