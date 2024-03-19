using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<GameObject> cardChatsMatch = new List<GameObject>();
    [Header("Panels")]
    public GameObject panelMain;
    public GameObject panelTinder;
    public GameObject panelChat;
    public GameObject panelNewGame;
    private GameObject panelActual;
    [Header("PlayGame")]
    public TMP_Text playText;
    [Header("Alert")]
    public GameObject alertTinder;
    public GameObject alertChat;
    [Header("Chat")]
    public Image imageFaceGirl;
    public int valueAddChatRoll;
    public GameObject prefabCharGirl;
    public GameObject prefabChatMain;
    public GameObject prefabImageChatGirl;
    public Transform targetChatGirl;
    public Transform targetChatMain;
    public Transform targetResponse;
    public Image imageZoom;
    [Header("Tinder")]
    public Transform targetCardChats;
    public List<GameObject> cardChats;
    public CardChat cardChatActual;

    [Header("Notification")]
    public GameObject panelNotification;
    public TMP_Text messageNotification;
    // Start is called before the first frame update
    void Start()
    {
        ClosePanels();
        ActivePanelNewGame();
    }

    public void StartGame()
    {
        SetPanel(panelMain);
    }

    private void SetPanel(GameObject panel)
    {
        if (panelActual != null)
        {
            panelActual.SetActive(false);
        }

        panelActual = panel;
        panelActual.SetActive(true);
    }

    private void ActivePanelNewGame()
    {
        SetPanel(panelNewGame);
        playText.gameObject.LeanScale(new Vector3(1.2f, 1.2f), 0.35f).setLoopPingPong();
    }

    public void StartTinder()
    {
        if (!VerifyUsedCars()) FinishTinder();
        SetPanel(panelTinder);
        DrawingCard();
    }

    public void StartChat()
    {
        SetPanel(panelChat);
    }

    public void ClosePanel()
    {
        SetPanel(panelMain);
    }

    private void ClosePanels()
    {
        panelMain.SetActive(false);
        panelTinder.SetActive(false);
        panelChat.SetActive(false);
        panelNewGame.SetActive(false);
    }

    #region Chat

    public void CloseImageZoom()
    {
        imageZoom.gameObject.SetActive(false);
    }

    public void SelectedBubble(string textMessage)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Tinder
    public void Liked()
    {
        var randomNumber = Random.Range(0, 100);
        if (randomNumber > 49)
        {
            Debug.Log($"MATCHED");
            cardChatActual.match = true;
            cardChatsMatch.Add(cardChatActual.gameObject);
            cardChatActual.gameObject.SetActive(false);
            cardChatActual = null;
            DrawingCard();
        }
        else
        {
           Disliked();
        }
    }

    public void Disliked()
    {
        cardChatActual.gameObject.SetActive(false);
        cardChatActual.used = true;
        cardChatActual = null;
        DrawingCard();
    }

    private void DrawingCard()
    {
        if (cardChats.Count <= 0)
        {
            FinishTinder();
        }
        else
        {
            if (!VerifyUsedCars()) SetNotification($"Wait for updates!!!");
            var indexCard = Random.Range(0, cardChats.Count - 1);
            var cardDrawing = Instantiate(cardChats[indexCard],targetCardChats);
            cardChats.Remove(cardChats[indexCard]);
            var card = cardDrawing.GetComponent<CardChat>();
            cardChatActual = card;
        }
    }

    private void FinishTinder()
    {
        SetPanel(panelMain);
    }
    

    private bool VerifyUsedCars()
    {
        foreach (var item in cardChats)
        {
            var card = item.GetComponent<CardChat>();
            if (!card.used)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region Notification

    private void SetNotification(string message)
    {
        messageNotification.text = message;
        panelNotification.SetActive(true);
    }

    public void CloseNotification()
    {
        panelNotification.SetActive(false);
        messageNotification.text = String.Empty;
    }

    #endregion
}
