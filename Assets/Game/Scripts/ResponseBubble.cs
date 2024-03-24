using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResponseBubble : MonoBehaviour
{
    public TMP_Text textMessage;
    public GameManager gameManager;
    public Responses response;
    public void SelectedBubbleResponse()
    {
        gameManager.AddMainResponse(textMessage.text);
        gameManager.CreatedChatGirl(gameManager.chatFaceGirlActual.chat.questions,response);
    }

    public void SetTextMessage(string message)
    {
        textMessage.text = "";
        textMessage.text = message;
    }

    public void SetResponse(Responses responses)
    {
        this.response = responses;
    }
}
