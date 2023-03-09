using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInteractableHandler : MonoBehaviour {

    public Queue<KeyAndTargetPosition> Targets;

    private Table getCurrentTable = null;
    private PlayerController playerController = null;
    private bool reachedTargetPosition = false;
    private bool reachedOriginalPosition = false;
    private bool goDirectToOriginal = false;
    private bool dontExecuteMove = false;
    private bool availableForNextTarget = true;

    void Start() {
        //if (Instance == null)
        //    Instance = this;
        //else if (Instance != this)
        //    Destroy(gameObject);

        playerController = GameLevelManager.Instance.GetPlayerController();
        Targets = new Queue<KeyAndTargetPosition>();
        reachedOriginalPosition = reachedTargetPosition = false;
    }

    public bool SetTargets(InteractableObject interactableObject, Vector3 targetPosition, Vector3 originalPosition) {
        if (Check_DuplicatePosition(interactableObject)) {
            goDirectToOriginal = true;
            if(interactableObject is Menu)
                dontExecuteMove = true;
        }
        playerController.playerInfo.SetPlayerKeyLocation(interactableObject.GetKeyName, "PlayerMove@InputInteractableHandler.cs");

        interactableObject.SetCheckActiveOn(interactableObject.GetClickCount() - 1);
        Targets.Enqueue(new KeyAndTargetPosition(interactableObject, targetPosition, originalPosition, goDirectToOriginal, dontExecuteMove));
        goDirectToOriginal = dontExecuteMove = false;
        return true;
    }

    private bool Check_DuplicatePosition(InteractableObject interactableObject) {
        int targetsSize = Targets.Count;
        string compareGetKeyName = playerController.playerInfo.GetPlayerKeyLocation();

        if (Targets.Count > 1) {
            compareGetKeyName = Targets.ToArray()[targetsSize - 1].interactableObject.GetKeyName;
        }

        if (interactableObject.GetKeyName == compareGetKeyName) {
            return true;
        }

        return false;
    }

    private void Update() {
        if(Targets.Count != 0) {
            if (availableForNextTarget) {
                StartCoroutine(PlayerMove());
            }
        }
        
    }

    private IEnumerator PlayerMove() {
        if(Targets.Count <= 0) {
            yield break;
        }
        
        availableForNextTarget = false;
        if (!Targets.Peek().dontExecuteMove) {
            while (!reachedOriginalPosition) {
                if ( Targets.Peek().goToOrigTarget || (reachedTargetPosition && !playerController.playerMovement.moving)) { //if player is in the target position AND player not moving (target has been reached), go to original
                    if(playerController.playerInfo.GetPlayerState() != PlayerState.WALKING) {
                        playerController.playerMovement.Player_GoTo(Targets.Peek().originalPosition);
                    }
                    if (!playerController.playerMovement.reachingNewPosition) {
                        reachedTargetPosition = false;
                        goDirectToOriginal = false;
                        reachedOriginalPosition = true;
                    }
                } else if (!playerController.playerMovement.moving && !reachedOriginalPosition) { //If player is not moving, go to target position
                    playerController.playerMovement.Player_GoTo(Targets.Peek().targetPosition, true); //true if you don't want to move to next queue
                    playerController.playerInfo.SetPlayerKeyLocation(Targets.Peek().interactableObject.GetKeyName, "PlayerMove@InputInteractableHandler.cs");
                    reachedTargetPosition = true;
                }
                yield return new WaitForEndOfFrame();
            }
        }
        SetInteractions();
        Targets.Peek().interactableObject.SetCheckActiveOff();
        Targets.Peek().interactableObject.SetClickCount(0);
        Targets.Dequeue();
        reachedOriginalPosition = false;
        availableForNextTarget = true;
        StopCoroutine(PlayerMove());
    }

    private void SetInteractions() {
        if (Targets.Peek().interactableObject is Table) { 
            TableInteraction();
        } else if (Targets.Peek().interactableObject is Menu) { 
            MenuInteraction();
        } else if (Targets.Peek().interactableObject is Food) {
            FoodInteraction();
        }
        playerController.playerMovement.updateIdle();
    }

    private void TableInteraction() {
        getCurrentTable = (Table)Targets.Peek().interactableObject;
        if (getCurrentTable.GetCurrentTableState() == Table.TableState.OCCUPIED) {
            if (playerController.playerInfo.IsPlayerHoldingFood()) {
                int getPlayerFood_HandPosition = 0;
                bool isFoodForTable = false;
                Food[] foods = playerController.playerInfo.GetFoods();
                for (int i = 0; i < foods.Length; i++) {
                    if(foods[i] == null) {
                        continue;
                    }
                    if (foods[i].foodTableId == getCurrentTable.tableId) {
                        getPlayerFood_HandPosition = i;
                        isFoodForTable = true;
                        break;
                    }
                }

                //must notify customerBatch player arrival for taking their order while holding a food (Player must notify table even when food isn't for the table)
                if (!isFoodForTable) {
                    //if food isn't for the table and player has two items carry, assume that player cant do anything
                    if (playerController.playerInfo.GetCarryCount() == PlayerStats.playerCanHold) {
                        return;
                    }
                    //if food isn't for the table and customer is currently waiting for their food
                    if(getCurrentTable.GetCurrentCustomer_InfoHandler().customerInfo.GetCustomerCurrentState() == CustomerState.WAITING_FOR_FOOD) {
                        return;
                    }
                    getCurrentTable.NotifyCustomerBatch_PlayerArrival();
                    return;
                }

                int getPlayerCurrentHold_Count = playerController.playerInfo.GetCarryCount();
                if (playerController.playerInfo.GetFood(getPlayerFood_HandPosition) != null) {
                    playerController.playerInfo.GetFood(getPlayerFood_HandPosition).DestroyFood();
                    //if you are holding two dishes and the first dish you are holding will be served. Update dish 2nd position
                    if(getPlayerFood_HandPosition == 0 && playerController.playerInfo.GetPlayerState() == PlayerState.HOLDING_FOOD_2) {
                        playerController.playerInfo.SetFood(playerController.playerInfo.GetFood(1), 0); //switching the 2nd food to 1st array
                        playerController.playerInfo.SetFood(null, 1);
                        playerController.playerInfo.GetFood(0).SetLocalPosition_WhenInPlayer(0, playerController.playerMovement.arm);
                    }
                    getPlayerCurrentHold_Count--;
                }
                playerController.playerInfo.SetCarryCount(getPlayerCurrentHold_Count);

                getCurrentTable.NotifyCustomerBatch_PlayerArrival();

                if (getPlayerCurrentHold_Count == 1) {
                    if (playerController.playerInfo.GetPlayerState() == PlayerState.HOLDING_FOOD_ORDER) {
                        UpdatePlayerHoldPosition();
                        playerController.playerInfo.SetPlayerState(PlayerState.HOLDING_ORDER, "InputInteractableHandler.cs");
                        return;
                    }
                    playerController.playerInfo.SetPlayerState(PlayerState.HOLDING_FOOD, "InputInteractableHandler.cs");
                } else if(getPlayerCurrentHold_Count == 0) {
                    playerController.playerInfo.SetPlayerState(PlayerState.IDLE, "InputInteractableHandler.cs");
                }

                return;
            }
            //if player has two carry items and customer is currently order ready (Player must notify customer even when it has two order carry when the customer is paying)
            if (getCurrentTable.GetCurrentCustomer_InfoHandler().customerInfo.GetCustomerCurrentState() == CustomerState.ORDER_READY &&
                playerController.playerInfo.GetCarryCount() == PlayerStats.playerCanHold) {
                return;
            }
        }
        getCurrentTable.NotifyCustomerBatch_PlayerArrival();
    }
    private void MenuInteraction() {
        if (playerController.playerInfo.IsPlayerHoldingOrder()) {
            Menu getCurrentMenu = (Menu)Targets.Peek().interactableObject;

            int noteIndex = 0;
            bool switchOrder2ToFirstArray = false;
            Note getPlayerNote = playerController.playerInfo.GetNote(0);
            switch (playerController.playerInfo.GetPlayerState()) {
                case PlayerState.HOLDING_ORDER_2:
                    switchOrder2ToFirstArray = true;
                    break;
                case PlayerState.HOLDING_FOOD_ORDER:
                    if(playerController.playerInfo.GetNote(1) != null) {
                        noteIndex = 1;
                        getPlayerNote = playerController.playerInfo.GetNote(1);
                    }
                    break;
            }
            getPlayerNote.customerBatch.GetCustomerInfoHandler().SetCheckPatienceActive(false); //when player put the order to the order pin. Patience must not move
            getCurrentMenu.SetCustomerOrder(getPlayerNote.noteId, getPlayerNote.customerBatch); //Put note on the menu
            if (playerController.playerInfo.GetNote(noteIndex) != null) {
                playerController.playerInfo.GetNote(noteIndex).DestroyNote();
            }

            if (switchOrder2ToFirstArray) {
                playerController.playerInfo.SetNote(playerController.playerInfo.GetNote(1), 0); //switching the 2nd note to 1st array
                playerController.playerInfo.SetNote(null, 1);
                playerController.playerInfo.GetNote(0).SetLocalPosition_WhenInPlayer(0, playerController.playerMovement.arm);
            }


            if (playerController.playerInfo.GetCarryCount() == 1) {
                if(playerController.playerInfo.GetPlayerState() == PlayerState.HOLDING_FOOD_ORDER) {
                    UpdatePlayerHoldPosition();
                    playerController.playerInfo.SetPlayerState(PlayerState.HOLDING_FOOD, "InputInteractableHandler.cs");
                    return;
                }
                playerController.playerInfo.SetPlayerState(PlayerState.HOLDING_ORDER, "InputInteractableHandler.cs");
            } else {
                playerController.playerInfo.SetPlayerState(PlayerState.IDLE, "InputInteractableHandler.cs");
            }
        }
    }
    private void FoodInteraction() {
        if (playerController.playerInfo.GetPlayerState() == PlayerState.HOLDING_FOOD_2 || playerController.playerInfo.GetPlayerState() == PlayerState.HOLDING_FOOD_ORDER || playerController.playerInfo.GetPlayerState() == PlayerState.HOLDING_ORDER_2) {
            return;
        }
        Food getCurrentFood = (Food)Targets.Peek().interactableObject;
        if(getCurrentFood.GetCheckObject()) {
            int setPlaceOfFood = playerController.playerInfo.GetCarryCount(); //if player has no carry, it will go to 1st position. else, 2nd position
            getCurrentFood.SetLocalPosition_WhenInPlayer(setPlaceOfFood, playerController.playerMovement.arm);
            getCurrentFood.SetClickable(false);
            

            playerController.playerInfo.SetCarryCount(++setPlaceOfFood);
            FindObjectOfType<FoodSpawnHandler>().SetFoodSpawnSpot_Available(getCurrentFood.foodSpawnId); //temp
            playerController.playerInfo.SetFood(getCurrentFood, setPlaceOfFood - 1);

            if (playerController.playerInfo.GetCarryCount() == 1) {
                playerController.playerInfo.SetPlayerState(PlayerState.HOLDING_FOOD, "InputInteractableHandler.cs");
            } else {
                if (playerController.playerInfo.GetPlayerState() == PlayerState.HOLDING_ORDER) {
                    playerController.playerInfo.SetPlayerState(PlayerState.HOLDING_FOOD_ORDER, "InputInteractableHandler.cs");
                    return;
                }
                playerController.playerInfo.SetPlayerState(PlayerState.HOLDING_FOOD_2, "InputInteractableHandler.cs");
            }
        }
    }

    private void UpdatePlayerHoldPosition() {
        if(playerController.playerInfo.GetFood(0) != null) {
            return;
        }

        if (playerController.playerInfo.GetNote(0) != null) {
            return;
        }

        if (playerController.playerInfo.GetFood(1) != null) {
            playerController.playerInfo.GetFood(1).SetLocalPosition_WhenInPlayer(0, playerController.playerMovement.arm);
        } else if (playerController.playerInfo.GetNote(1) != null) {
            playerController.playerInfo.GetNote(1).SetLocalPosition_WhenInPlayer(0, playerController.playerMovement.arm);
        }
        playerController.playerMovement.updateIdle();
    }

    public struct KeyAndTargetPosition {

        public InteractableObject interactableObject;
        public Vector3 targetPosition;
        public Vector3 originalPosition;
        public bool goToOrigTarget;
        public bool dontExecuteMove;

        public KeyAndTargetPosition(InteractableObject interactableObject, Vector3 targetPosition, Vector3 originalPosition, bool goToOrigTarget, bool dontExecuteMove) {
            this.interactableObject = interactableObject;
            this.targetPosition = targetPosition;
            this.originalPosition = originalPosition;
            this.goToOrigTarget = goToOrigTarget;
            this.dontExecuteMove = dontExecuteMove;
        }
    }
}
