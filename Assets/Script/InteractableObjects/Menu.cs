using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : InteractableObject {

    [SerializeField]
    private FoodSpawnHandler foodSpawnHandler;

    private PlayerController playerController = null;


    public override void Start() {
        base.Start();
        playerController = GameLevelManager.Instance.GetPlayerController();
        SetCheckActiveOff();
        LayerManager.instance.updateLayer(transform);
    }

    public void SetCustomerOrder(int orderId, CustomerBatch customerBatch) {
        playerController.playerInfo.SetCarryCount(playerController.playerInfo.GetCarryCount() - 1);
        if(customerBatch != null) {
           foodSpawnHandler.SetFood(orderId, customerBatch);
        }
    }


}
