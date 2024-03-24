using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChatImageFaceGirl : MonoBehaviour
{
   public Image imageFaceGirl;
   public GameObject imageNotification;
   public GameManager gameManager;
   public Image imageSelected;
   public Responses responseActual = Responses.RightResponse;
   public chat chat;
   public List<GameObject> questionsGirl;
   public List<GameObject> questionsMain;
   public List<GameObject> optionsResponse;
   public readonly List<ChatFaceGirl> chatFaceGirlSaved = new List<ChatFaceGirl>();
   
   public void ActiveNotification()
   {
      imageNotification.SetActive(true);
   }

   public void DisableNotification()
   {
      imageNotification.SetActive(false);
   }
   public void SelectedChat()
   {
      imageSelected.gameObject.SetActive(true);
      gameManager.SelectedFaceGirl(gameObject.name);
      gameManager.SetResponse(responseActual);
   }

   public void AddLIstQuestionGirl(GameObject message)
   {
      questionsGirl.Add(message);
   }

   public void AddListQuestionMain(GameObject message)
   {
      questionsMain.Add(message);
   }

   public void AddListOptionResponse(GameObject message)
   {
      optionsResponse.Add(message);
   }

   public void ClearListQuestionGirl()
   {
      foreach (var question in questionsGirl)
      {
         Destroy(question);
      }
      questionsGirl.Clear();
   }
   public void ClearListQuestionMain()
   {
      foreach (var question in questionsMain)
      {
         Destroy(question);
      }
      questionsMain.Clear();
   }
   public void ClearListOptionsResponse()
   {
      foreach (var question in optionsResponse)
      {
         Destroy(question);
      }
      optionsResponse.Clear();
   }

   public void SaveChat(string questionGirlActual, string questionMainActual, string optionRightActual, string optionWrongActual)
   {
      if (chatFaceGirlSaved.Count > 0)
      {
         var faceGirl = chatFaceGirlSaved[^1];
         if (faceGirl.chatGirl.Equals(questionGirlActual))
         {
            if (faceGirl.chatMain is "" or null)
            {
               faceGirl.chatMain = questionMainActual;
               return;
            }
         }
      }
      var chatFaceGirl = new ChatFaceGirl(
         questionGirlActual,
         questionMainActual,
         optionRightActual,
         optionWrongActual);
      chatFaceGirlSaved.Add(chatFaceGirl);
   }
}
