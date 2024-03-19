using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardChat : MonoBehaviour
{
    public Image imageCard;
    public TMP_Text nameCard;
    public bool match;
    public string nameGirlCard;
    public Sprite faceGirlCard;
    public List<chat> chat;
    public int percentageMatch;
    public bool used;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[Serializable]
public class chat
{
    public int id;
    public bool used;
    public bool match;
    public List<questions> questions;
    public List<photos> photos;
}
[Serializable]
public class questions
{
    public string rightQuestions;
    public string wrongQuestions;
    public string rightResponse;
    public string wrongResponse;
    public bool used;
}
[Serializable]
public class photos
{
    public Sprite sexyImage;
    public Sprite hotImage;
    public bool used;
}
