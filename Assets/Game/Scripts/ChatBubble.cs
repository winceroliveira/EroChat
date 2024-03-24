using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    public TMP_Text textMessage;

    public void SetMessageText(string message)
    {
        textMessage.text = "";
        textMessage.text = message;
    }
}
