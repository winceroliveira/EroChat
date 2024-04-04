using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;
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
    public GameObject panelGallery;
    private GameObject panelActual;
    [Header("PlayGame")]
    public TMP_Text playText;
    public TMP_InputField inputName;
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
    public Sprite imageGirlActual;
    public ChatImageFaceGirl chatFaceGirlActual;
    public GameObject contentScrollView;
    private Responses response;
    [Header("Tinder")] 
    public GameObject panelChose;
    public Transform targetCardChats;
    public List<GameObject> cardChatsFemale;
    public List<GameObject> cardChatsMale;
    public List<GameObject> cardChatsTrans;
    public List<GameObject> cardChatsFurry;
    public List<GameObject> cardChatsAll;
    private List<GameObject> listCardsChosen;
    public CardChat cardChatActual;
    [Header("Notification")]
    public GameObject panelNotification;
    public TMP_Text messageNotification;
    [Header("Frame Player")] 
    public TMP_Text namePlayerText;
    public TMP_Text levelPlayerText;
    public TMP_Text experiencePlayer;
    public Slider levelPlayerSlider;
    private string namePlayer;
    private int levelPlayer = 1;
    private int experienceActual = 550;
    private int experienceNextLevel = 1000;
    [Header("Gallery")]
    public Image imageZoomGallery;
    

    
    #region GameManager

    private void Start()
    {
        ClosePanels();
        ActivePanelNewGame();
        SetLevel(1);
        experiencePlayer.text = $"{experienceActual}/{experienceNextLevel}";
        levelPlayerSlider.maxValue = experienceNextLevel;
        levelPlayerSlider.value = experienceActual;
    }

    public void StartGame()
    {
        if (inputName.text.Trim() == string.Empty)
        {
            SetNotification($"you need to enter a name");
            return;
        }

        namePlayer = inputName.text;
        namePlayerText.text = namePlayer;
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
        SetPanel(panelTinder);
        panelChose.SetActive(true);
        // DrawingCard();
    }

    public void ChosenGender(int gender)
    {
        var genderChose = (Genders)gender;
        switch (genderChose)
        {
            case Genders.Female:
                listCardsChosen = cardChatsFemale;
                break;
            case Genders.Male:
                listCardsChosen = cardChatsMale;
                break;
            case Genders.Trans:
                listCardsChosen = cardChatsTrans;
                break;
            case Genders.Furry:
                listCardsChosen = cardChatsFurry;
                break;
            case Genders.All:
                listCardsChosen = cardChatsAll;
                break;
        }
        if (!VerifyUsedCards()) FinishTinder();
        panelChose.SetActive(false);
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
        imagesFaceGirlInstantiating[^1].GetComponent<ChatImageFaceGirl>().SelectedChat();
        // selectedChatActual.GetComponent<ChatImageFaceGirl>().SelectedChat();
        // CreatedChatGirl(imagesFaceGirlInstantiating[^1].GetComponent<ChatImageFaceGirl>().chat.questions);
    }

    public void StartGallery()
    {
        SetPanel(panelGallery);
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
    #region Player

    private void SetLevel(int level)
    {
        levelPlayer += level;
        levelPlayerText.text = $"{levelPlayer}";
        if (experienceActual >= experienceNextLevel)
        {
            SetExperience();
        }
    }

    private void SetExperience(int experience = 0)
    {
        experienceActual += experience;
        if (experienceActual < experienceNextLevel) return;
        experienceActual -= experienceNextLevel;
        experienceNextLevel *= 2;
        experiencePlayer.text = $"{experienceActual}/{experienceNextLevel}";
        levelPlayerSlider.maxValue = experienceNextLevel;
        levelPlayerSlider.value = experienceActual;
        SetLevel(1);
    }

    #endregion
    
    #region Chat

    public void CloseImageZoom()
    {
        imageZoom.gameObject.SetActive(false);
        imageZoomGallery.gameObject.SetActive(false);
    }
    private void ClearChats()
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
    private void DeselectImageChats()
    {
        foreach (var obj in imagesFaceGirlInstantiating)
        {
            obj.GetComponent<ChatImageFaceGirl>().DeselectChat();
        }
    }
    public void SelectedFaceGirl(string nameGameObj)
    {
        ClearChats();
        DeselectImageChats();
        var position = contentScrollView.transform.localPosition;
        position = new Vector3(position.x,0,position.z);
        contentScrollView.transform.localPosition = position;
        if (chatFaceGirlActual != null)
        {
            // chatFaceGirlActual.DeselectChat();
        }
        var chatImageActual = imagesFaceGirlInstantiating.Find(x => x.name == nameGameObj);
        var chatImageFaceGirl = chatImageActual.GetComponent<ChatImageFaceGirl>();
        chatFaceGirlActual = chatImageFaceGirl;
        chatFaceGirlActual.ActivateImageSelected();
        var chatFaceGirlSaved = chatFaceGirlActual.chatFaceGirlSaved;
        if (chatFaceGirlSaved.Count <= 0) return;
        foreach (var chat in chatFaceGirlSaved)
        {
            var chatFaceGirl = Instantiate(prefabChatGirl, targetChatGirl);
            chatFaceGirl.GetComponent<ChatBubble>().SetMessageText(chat.chatGirl);
            chatFaceGirlActual.AddLIstQuestionGirl(chatFaceGirl);
            if (chat.chatMain != string.Empty)
            {
                var chatMain = Instantiate(prefabChatMain, targetChatMain);
                chatMain.GetComponent<ChatBubble>().SetMessageText(chat.chatMain);
                chatFaceGirlActual.AddListQuestionMain(chatMain);
            }

            if (chat.imageGirl != null)
            {
                var imageGirl = Instantiate(prefabImageChatGirl, targetChatGirl);
                var chatImage = imageGirl.GetComponent<ChatImage>();
                chatImage.gameManager = this;
                chatImage.imageGirl.sprite = chat.imageGirl;
                chatFaceGirlActual.AddLIstQuestionGirl(imageGirl);
            }
        }

        if (chatFaceGirlSaved[^1].rightResponse == string.Empty) return;
        var rightResponse = chatFaceGirlSaved[^1].rightResponse;
        InstantiateResponsesChat(rightResponse, Responses.RightResponse);
        var wrongResponse = chatFaceGirlSaved[^1].wrongResponse;
        InstantiateResponsesChat(wrongResponse, Responses.WrongResponse);
        
        return;

        void InstantiateResponsesChat(string message, Responses responses)
        {
            var responseBubble = Instantiate(prefabResponseChat, targetResponse).GetComponent<ResponseBubble>();
            responseBubble.gameManager = this;
            responseBubble.SetResponse(responses);
            responseBubble.SetTextMessage(message);
            chatFaceGirlActual.AddListOptionResponse(responseBubble.gameObject);
        }
    }
    private void AddChat(GameObject obj)
    {
        var cardChat = obj.GetComponent<CardChat>();
        var chatFace = Instantiate(prefabFaceGirl, targetFaceGirl).GetComponentInChildren<ChatImageFaceGirl>();
        chatFace.imageFaceGirl.sprite = cardChat.faceGirlCard;
        chatFace.gameObject.name = $"{chatFace.imageFaceGirl.sprite.name}";
        chatFace.ActiveNotification();
        chatFace.chat = cardChat.chat[0];
        chatFace.gameManager = this;
        chatFaceGirlActual = chatFace;
        imagesFaceGirlInstantiating.Add(chatFace.gameObject);
        CreatedChatGirl(chatFace.chat.questions);
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

        // ReSharper disable once LocalFunctionHidesMethod
        void SetResponse(string message)
        {
            chatBubble.SetMessageText(message);
            questionGirlActual = message;
            chatFaceGirlActual.AddLIstQuestionGirl(chatBubble.gameObject);
        }
    }
    public void CreatedChatGirl(List<questions>questionsList, Responses responses, List<photos> photosList)
    {
        if (AllUsed(questionsList))
        {
            chatFaceGirlActual.ClearListOptionsResponse();
            return;
        }
        var chatBubble = Instantiate(prefabChatGirl, targetChatGirl).GetComponent<ChatBubble>();
        foreach (var question in questionsList.Where(question => !question.used))
        {
            question.used = true;
            switch (responses)
            {
                case Responses.RightResponse:
                    SetResponse(question.rightQuestions);
                    SetImage();
                    break;
                case Responses.WrongResponse:
                    SetResponse(question.wrongResponse);
                    SetImage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            AddResponses(question.rightResponse,question.wrongResponse);
            break;
        }
        
        return;

        // ReSharper disable once LocalFunctionHidesMethod
        void SetResponse(string message)
        {
            chatBubble.SetMessageText(message);
            questionGirlActual = message;
            chatFaceGirlActual.AddLIstQuestionGirl(chatBubble.gameObject);
        }

        void SetImage()
        {
            var hit = Random.Range(0, 100);
            if (hit <= 40)
            {
                imageGirlActual = null;
                return;
            }
            foreach (var photo in photosList.Where(photo => !photo.used))
            {
                photo.used = true;
                var imageGirl = Instantiate(prefabImageChatGirl, targetChatGirl);
                var chatImage = imageGirl.GetComponent<ChatImage>();
                chatImage.gameManager = this;
                chatImage.imageGirl.sprite = responses == Responses.RightResponse ? photo.hotImage : photo.sexyImage;
                chatFaceGirlActual.AddLIstQuestionGirl(imageGirl);
                imageGirlActual = chatImage.imageGirl.sprite;
                break;
            }
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
        chatFaceGirlActual.SaveChat(questionGirlActual, questionMainActual, optionRightActual, optionWrongActual,imageGirlActual);
    }
    public void AddMainResponse(string message)
    {
        var chatBubble = Instantiate(prefabChatMain, targetChatMain).GetComponent<ChatBubble>();
        chatBubble.SetMessageText(message);
        questionMainActual = message;
        chatFaceGirlActual.AddListQuestionMain(chatBubble.gameObject);
        // chatFaceGirlActual.SaveChat(questionGirlActual, questionMainActual, optionRightActual, optionWrongActual);
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
    private static bool AllUsed(List<questions> list)
    {
        foreach (var item in list)
        {
            if (!item.used)
            {
                return false;
            }
        }
        return true;
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
        if (listCardsChosen.Count <= 0)
        {
            FinishTinder();
        }
        else
        {
            if (!VerifyUsedCards()) SetNotification($"Wait for updates!!!");
            var indexCard = Random.Range(0, listCardsChosen.Count - 1);
            var cardDrawing = Instantiate(listCardsChosen[indexCard],targetCardChats);
            listCardsChosen.Remove(listCardsChosen[indexCard]);
            var card = cardDrawing.GetComponent<CardChat>();
            cardChatActual = card;
        }
    }

    private void FinishTinder()
    {
        SetPanel(panelMain);
        SetNotification($"We currently have no more members for this category.");
    }
    
    private bool VerifyUsedCards()
    {
        foreach (var item in listCardsChosen)
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
[Serializable]
public class ChatFaceGirl
{
    public string chatGirl;
    public string chatMain;
    public string rightResponse;
    public string wrongResponse;
    public Sprite imageGirl;
    public ChatFaceGirl(string chatGirl, string chatMain, string rightResponse, string wrongResponse, Sprite imageGirl = null)
    {
        this.chatGirl = chatGirl;
        this.chatMain = chatMain;
        this.rightResponse = rightResponse;
        this.wrongResponse = wrongResponse;
        this.imageGirl = imageGirl;
    }

}
public enum Responses
{
    RightResponse,
    WrongResponse
}

public enum Genders
{
    Female,
    Male,
    Trans,
    Furry,
    All
}