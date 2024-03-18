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
        gameManager.imageZoom.sprite = imageGirl.sprite;
        gameManager.imageZoom.preserveAspect = true;
        gameManager.imageZoom.gameObject.SetActive(true);
    }
}
