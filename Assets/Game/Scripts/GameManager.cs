using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject selectedChatActual;
    public int valueAddChatRoll;
    public GameObject prefabChatGirl;
    public GameObject prefabChatMain;
    public GameObject prefabImageChatGirl;
    public GameObject prefabResponseChat;
    public GameObject prefabFaceGirl;
    public Transform targetChatGirl;
    public Transform targetChatMain;
    public Transform targetResponse;
    public Transform targetFaceGirl;
    public Image imageZoom;
    public List<GameObject> imagesFaceGirlInstantiating = new List<GameObject>();
    public List<GameObject> chatsGirlsInstantiating = new List<GameObject>();
    public List<GameObject> chatsMainInstantiating = new List<GameObject>();
    public List<GameObject> chatsResponses = new List<GameObject>();
    public List<GameObject> chatsFaceGirls = new List<GameObject>();
    public string questionGirlActual;
    public string questionMainActual;
    public string optionRightActual;
    public string optionWrongActual;
    public ChatImageFaceGirl chatFaceGirlActual;
    private Responses response;
    [Header("Tinder")]
    public Transform targetCardChats;
    public List<GameObject> cardChats;
    public CardChat cardChatActual;

    [Header("Notification")]
    public GameObject panelNotification;
    public TMP_Text messageNotification;

    #region GameManager
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
        alertTinder.SetActive(false);
        if (!VerifyUsedCars()) FinishTinder();
        SetPanel(panelTinder);
        DrawingCard();
    }

    public void StartChat()
    {
        if (cardChatsMatch.Count <= 0)
        {
            SetNotification($"you need a match to access the chat");
            return;
        }
        alertChat.SetActive(false);
        SetPanel(panelChat);
        selectedChatActual = cardChatsMatch[0];
        ClearChats();
        CreatedChatGirl(imagesFaceGirlInstantiating[^1].GetComponent<ChatImageFaceGirl>().chat.questions);
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
    #endregion
    
    #region Chat

    public void CloseImageZoom()
    {
        imageZoom.gameObject.SetActive(false);
    }

    //adicionar a foto na direita ok
    //quando abrir o painel de chat automaticamente tem que selecionar a primeira foto de cima 
    //quando a foto estiver selecionada tem que limpar tudo do chat e mostrar o dialogo daquela foto com as opçoes de resposta do player
    public void ClearChats()
    {
        foreach (var chatFaceGirl in imagesFaceGirlInstantiating)
        {
            var chatFace = chatFaceGirl.GetComponent<ChatImageFaceGirl>();
            chatFace.ClearListQuestionGirl();
            chatFace.ClearListQuestionMain();
            chatFace.ClearListOptionsResponse();
            // Destroy(chatFace.imageNotification.gameObject);
        }
        // imagesFaceGirlInstantiating.Clear();
    }
    public void SelectedFaceGirl(string nameGameObj)
    {
        var chatImageActual = imagesFaceGirlInstantiating.Find(x => x.name == nameGameObj);
        var chatImageFaceGirl = chatImageActual.GetComponent<ChatImageFaceGirl>();
        // chatImageFaceGirl.ClearListQuestionGirl();
        // chatImageFaceGirl.ClearListQuestionMain();
        // chatImageFaceGirl.ClearListOptionsResponse();
        chatFaceGirlActual = chatImageFaceGirl;
        var chatFaceGirl = chatFaceGirlActual.chatFaceGirlSaved;
        if (chatFaceGirl.Count <= 0) return;
        foreach (var chat in chatFaceGirl)
        {
            var chatFace = Instantiate(prefabChatGirl, targetChatGirl);
            chatFace.GetComponent<ChatBubble>().SetMessageText(chat.chatGirl);
            chatFaceGirlActual.AddLIstQuestionGirl(chatFace);
            var chatMain = Instantiate(prefabChatMain, targetChatMain);
            chatFace.GetComponent<ChatBubble>().SetMessageText(chat.chatMain);
            chatFaceGirlActual.AddListQuestionMain(chatMain);
        }

        if (chatFaceGirl[^1].rightResponse == string.Empty) return;
        var rightResponse = chatFaceGirl[^1].rightResponse;
        InstantiateResponsesChat(rightResponse, Responses.RightResponse);
        var wrongResponse = chatFaceGirl[^1].wrongResponse;
        InstantiateResponsesChat(wrongResponse, Responses.WrongResponse);

        return;

        void InstantiateResponsesChat(string message, Responses responses)
        {
            var responseBubble = Instantiate(prefabResponseChat, targetResponse).GetComponent<ResponseBubble>();
            responseBubble.gameManager = this;
            responseBubble.SetResponse(responses);
            responseBubble.SetTextMessage(message);
        }
    }
    private void AddChat(GameObject obj)
    {
        var cardChat = obj.GetComponent<CardChat>();
        var chatFace = Instantiate(prefabFaceGirl, targetFaceGirl).GetComponentInChildren<ChatImageFaceGirl>();
        chatFace.imageFaceGirl.sprite = cardChat.faceGirlCard;
        chatFace.ActiveNotification();
        chatFace.chat = cardChat.chat[0];
        chatFace.gameManager = this;
        chatFaceGirlActual = chatFace;
        imagesFaceGirlInstantiating.Add(chatFace.gameObject);
        // CreatedChatGirl(chatFace.chat.questions);
    }

    private void CreatedChatGirl(List<questions>questionsList)
    {
        var chatBubble = Instantiate(prefabChatGirl, targetChatGirl).GetComponent<ChatBubble>();
        foreach (var question in questionsList.Where(question => !question.used))
        {
            question.used = true;
            switch (response)
            {
                case Responses.RightResponse:
                    SetResponse(question.rightQuestions);
                    break;
                case Responses.WrongResponse:
                    SetResponse(question.wrongResponse);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            AddResponses(question.rightResponse,question.wrongResponse);
            break;
        }
        return;

        void SetResponse(string message)
        {
            chatBubble.SetMessageText(message);
            questionGirlActual = message;
            chatFaceGirlActual.AddLIstQuestionGirl(chatBubble.gameObject);
        }
    }
    public void CreatedChatGirl(List<questions>questionsList, Responses responses)
    {
        var chatBubble = Instantiate(prefabChatGirl, targetChatGirl).GetComponent<ChatBubble>();
        foreach (var question in questionsList.Where(question => !question.used))
        {
            question.used = true;
            switch (responses)
            {
                case Responses.RightResponse:
                    SetResponse(question.rightQuestions);
                    break;
                case Responses.WrongResponse:
                    SetResponse(question.wrongResponse);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            AddResponses(question.rightResponse,question.wrongResponse);
            break;
        }
        return;

        void SetResponse(string message)
        {
            chatBubble.SetMessageText(message);
            questionGirlActual = message;
            chatFaceGirlActual.AddLIstQuestionGirl(chatBubble.gameObject);
        }
    }

    private void AddResponses(string rightResponse, string wrongResponse)
    {
        optionRightActual = rightResponse;
        optionWrongActual = wrongResponse;
        var numberSort = Random.Range(0, 1);
        if (numberSort == 0)
        {
            chatFaceGirlActual.ClearListOptionsResponse();
            InstantiateResponse(rightResponse, Responses.RightResponse);
            InstantiateResponse(wrongResponse, Responses.WrongResponse);
        }
        else
        {
            chatFaceGirlActual.ClearListOptionsResponse();
            InstantiateResponse(wrongResponse, Responses.WrongResponse);
            InstantiateResponse(rightResponse, Responses.RightResponse);
        }
        chatFaceGirlActual.SaveChat(questionGirlActual, questionMainActual, optionRightActual, optionWrongActual);
    }

    public void AddMainResponse(string message)
    {
        var chatBubble = Instantiate(prefabChatMain, targetChatMain).GetComponent<ChatBubble>();
        chatBubble.SetMessageText(message);
        questionMainActual = message;
        chatFaceGirlActual.AddListQuestionMain(chatBubble.gameObject);
        chatFaceGirlActual.SaveChat(questionGirlActual, questionMainActual, optionRightActual, optionWrongActual);
    }

    private void InstantiateResponse(string message, Responses responses)
    {
        var responseBubble = Instantiate(prefabResponseChat, targetResponse).GetComponent<ResponseBubble>();
        responseBubble.gameManager = this;
        responseBubble.SetResponse(responses);
        responseBubble.SetTextMessage(message);
        chatFaceGirlActual.AddListOptionResponse(responseBubble.gameObject);
    }

    public void SetResponse(Responses responses)
    {
        this.response = responses;
    }
    #endregion

    #region Tinder
    public void Liked()
    {
        var randomNumber = Random.Range(0, 100);
        if (randomNumber > 20)
        {
            Debug.Log($"MATCHED");
            cardChatActual.match = true;
            cardChatsMatch.Add(cardChatActual.gameObject);
            AddChat(cardChatActual.gameObject);
            cardChatActual.gameObject.SetActive(false);
            cardChatActual = null;
            DrawingCard();
            alertChat.SetActive(true);
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

public class ChatFaceGirl
{
    public ChatFaceGirl(string chatGirl, string chatMain, string rightResponse, string wrongResponse)
    {
        this.chatGirl = chatGirl;
        this.chatMain = chatMain;
        this.rightResponse = rightResponse;
        this.wrongResponse = wrongResponse;
    }

    public string chatGirl {get; set;}
    public string chatMain {get; set;}
    public string rightResponse {get; set;}
    public string wrongResponse { get; set; }
}
public enum Responses
{
    RightResponse,
    WrongResponse
}