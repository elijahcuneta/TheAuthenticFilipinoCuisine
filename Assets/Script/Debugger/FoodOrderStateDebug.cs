using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOrderStateDebug : MonoBehaviour {

    [SerializeField]
    TextMesh[] FoodTM;

    [SerializeField]
    TextMesh[] OrderTM;

    private void Update() {
        FoodTM[0].text = "FoodCountLimit: " + GameLevelManager.Instance.GetPlayerController().playerInfo.GetFoods().Length.ToString();

        if (GameLevelManager.Instance.GetPlayerController().playerInfo.GetFood(0) == null) {
            FoodTM[1].text = "Food#1: " + "null";
        } else if (GameLevelManager.Instance.GetPlayerController().playerInfo.GetFood(0) != null) {
            FoodTM[1].text = "Food#1: " + GameLevelManager.Instance.GetPlayerController().playerInfo.GetFood(0).foodTableId.ToString();
        }
        if (GameLevelManager.Instance.GetPlayerController().playerInfo.GetFood(1) == null) {
            FoodTM[2].text = "Food#2: " + "null";
        } else if (GameLevelManager.Instance.GetPlayerController().playerInfo.GetFood(1) != null) {
            FoodTM[2].text = "Food#2: " + GameLevelManager.Instance.GetPlayerController().playerInfo.GetFood(1).foodTableId.ToString();
        }

        OrderTM[0].text = "OrderCountLimit: " + GameLevelManager.Instance.GetPlayerController().playerInfo.GetNotes().Length.ToString();

        if (GameLevelManager.Instance.GetPlayerController().playerInfo.GetNote(0) == null) {
            OrderTM[1].text = "Order#1: " + "null";
        } else if (GameLevelManager.Instance.GetPlayerController().playerInfo.GetNote(0) != null) {
            OrderTM[1].text = "Order#1: " + GameLevelManager.Instance.GetPlayerController().playerInfo.GetNote(0).noteId.ToString();
        }
        if (GameLevelManager.Instance.GetPlayerController().playerInfo.GetNote(1) == null) {
            OrderTM[2].text = "Order#2: " + "null";
        } else if (GameLevelManager.Instance.GetPlayerController().playerInfo.GetNote(1) != null) {
            OrderTM[2].text = "Order#2: " + GameLevelManager.Instance.GetPlayerController().playerInfo.GetNote(1).noteId.ToString();
        }
    }

}
