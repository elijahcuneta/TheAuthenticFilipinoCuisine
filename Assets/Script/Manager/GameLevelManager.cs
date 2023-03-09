using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Holder of all Manager in the Game Level
 * 
 */

public class GameLevelManager : MonoBehaviour {

    public static GameLevelManager Instance = null;

    [SerializeField] private InputInteractableHandler inputInteractableHandler = null;
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private CustomerHandler customerHandler = null;
    [SerializeField] private GameStateManager gameStateManager = null;
    [SerializeField] private HUDManager hudManager = null;
    [SerializeField] private TableSetManager tableSetManager = null;

    private void Awake() {
        if (Instance != this)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public InputInteractableHandler GetInputInteractableManager() {
        return inputInteractableHandler;
    }
    public PlayerController GetPlayerController() {
        return playerController;
    }
    public CustomerHandler GetCustomerHandler() {
        return customerHandler;
    }
    public GameStateManager GetGameStateManager() {
        return gameStateManager;
    }
    public HUDManager GetHUDManager() {
        return hudManager;
    }
    public TableSetManager GetTableSetManager() {
        return tableSetManager;
    }

}
