using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    IDLE = 1,
    WALKING = 2,
    HOLDING_ORDER = 3,
    HOLDING_FOOD = 4,
    HOLDING_ORDER_2 = 5,
    HOLDING_FOOD_2 = 6,
    HOLDING_FOOD_ORDER = 7, //holding both food and order
}

public class PlayerInfo : MonoBehaviour {

    private string playerKeyNameLocation = "StartingPosition";
    private PlayerState playerState = PlayerState.IDLE;

    private bool playerHoldingFood = false;
    private bool playerHoldingOrder = false;

    private Note[] currentNotes = null;
    private Food[] currentFoods = null;

    private int carryCount = 0;

    //debuggin
    private PlayerState newPlayerState = PlayerState.IDLE;

    void Awake() {
        Initialize();
    }

    void Initialize() {
        playerState = PlayerState.IDLE;
        currentNotes = new Note[2];
        currentFoods = new Food[2];
    }

    public PlayerState GetPlayerState() {
        return playerState;
    }
    public void SetPlayerState(PlayerState newPlayerState, string whoCalledIt) 
    {
        //Debug.Log("I'm the one who changed the PlayerState: " + whoCalledIt + "\nFrom: " + playerState.ToString() + " to: " + newPlayerState.ToString());
        playerState = newPlayerState;
        UpdatePlayerState();
    }

    public string GetPlayerKeyLocation() {
        return playerKeyNameLocation;
    }
    public void SetPlayerKeyLocation(string newPlayerKeyNameLocation, string whoCalledIt) {
        //Debug.Log("I'm the one who changed the PlayerKeyNameLocation: " + whoCalledIt + "\nFrom: " + playerKeyNameLocation + " to: " + newPlayerKeyNameLocation);
        playerKeyNameLocation = newPlayerKeyNameLocation;
    }

    public void SetNote(Note newNoteId, int index) {
        currentNotes[index] = newNoteId;
    }
    public Note GetNote(int index) {
        return currentNotes[index];
    }

    public void SetFood(Food newFoodId, int index) {
        currentFoods[index] = newFoodId;
    }
    public Food GetFood(int index) {
        return currentFoods[index];
    }

    public Note[] GetNotes() {
        return currentNotes;
    }
    public Food[] GetFoods() {
        return currentFoods;
    }

    public void SetCarryCount(int newCarryCount) {
        if(newCarryCount < 0) {
            newCarryCount = 0;
        } else if(newCarryCount > PlayerStats.playerCanHold) {
            newCarryCount = PlayerStats.playerCanHold - 1;
        }
        carryCount = newCarryCount;
    }
    public int GetCarryCount() {
        return carryCount;
    }

    public bool IsPlayerHoldingFood() {
        return playerHoldingFood;
    }
    public bool IsPlayerHoldingOrder() {
        return playerHoldingOrder;
    }

    private void UpdatePlayerState() {
        playerHoldingFood = playerState == PlayerState.HOLDING_FOOD || playerState == PlayerState.HOLDING_FOOD_2 || playerState == PlayerState.HOLDING_FOOD_ORDER;
        playerHoldingOrder = playerState == PlayerState.HOLDING_ORDER || playerState == PlayerState.HOLDING_ORDER_2 || playerState == PlayerState.HOLDING_FOOD_ORDER;
    }

    private void Update() {
        if(newPlayerState != GetPlayerState()) {
            GameLevelManager.Instance.GetPlayerController().textMesh.text = "PlayerState:\n" + GetPlayerState().ToString();
            newPlayerState = GetPlayerState();
        }
    }
}
