using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        ActivePanelNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        SetPanel(panelTinder);
    }

    public void StartChat()
    {
        SetPanel(panelChat);
    }

    public void ClosePanel()
    {
        SetPanel(panelMain);
    }

    #region Chat

    public void CloseImageZoom()
    {
        imageZoom.gameObject.SetActive(false);
    }

    #endregion

    public void SelectedBubble(string textMessage)
    {
        throw new System.NotImplementedException();
    }
}
