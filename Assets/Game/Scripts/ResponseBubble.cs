using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResponseBubble : MonoBehaviour
{
    public TMP_Text textMessage;
    public GameManager gameManager;

    public void SelectedBubble()
    {
        gameManager.SelectedBubble(textMessage.text);
    }
}
