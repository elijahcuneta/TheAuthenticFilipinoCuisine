using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriviaOverlay : MonoBehaviour {

    [SerializeField]
    Image bg;
    [SerializeField]
    Text trivia;
    [SerializeField]
    GameObject nextButton, backButton;

    //void OnEnable () {
    //       bg.sprite = GameData.activeStage.background;
    //       trivia.text = GameData.activeStage.trivia;
    //       if(GameData.chosenLevel == 2 && GameData.activeStage.index != 5 && GameData.activeStage.index != 2) {
    //           nextButton.SetActive(true);
    //           backButton.SetActive(false);
    //       }else {
    //           nextButton.SetActive(false);
    //           backButton.SetActive(true);
    //       }
    //   }

    //   public void next() {
    //       trivia.text = GameData.activeStage.lastTrivia;
    //       nextButton.SetActive(false);
    //       backButton.SetActive(true);
    //   }
    void OnEnable() {
        bg.sprite = GameData.activeStage.background;
        trivia.text = GameData.activeStage.lastTrivia;
    }
}