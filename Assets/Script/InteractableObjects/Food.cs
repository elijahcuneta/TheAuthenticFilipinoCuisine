using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : InteractableObject {

    [HideInInspector]
    public int foodTableId = 1;

    [SerializeField]
    private SpriteRenderer tableIdSprite;

    [SerializeField]
    private CustomerOrderSprite[] customerOrderSprite = null;
    
    [HideInInspector]
    public int foodSpawnId = 0;

    [HideInInspector]
    public int handPosition = 0;

    [HideInInspector]
    public bool foodInHand = false;

    private int getCustomerCount = 0;

    public override void Start() {
        base.Start();
        SetAllOrderSpriteActive(false);
        SetCheckActiveOff();  
    }

    public void SetFoodId(int foodId, CustomerBatch customerBatch) {
        this.foodTableId = foodId;
        getCustomerCount = customerBatch.GetCustomerInfoHandler().customerInfo.customerCount;
        for (int i = 0; i < getCustomerCount; i++) {
            SetFoodSprite(customerBatch.GetCustomerOrderSprite(i), i);
        }
        SetTableIdSprite(customerBatch.GetCustomerTableIdSprite());
    }
    public void SetLocalPosition_WhenInPlayer(int index, Transform[] playerTransform = null) {
       
        if (playerTransform != null) {
            if (checkObject) {
                transform.parent = playerTransform[index];
            } else {
                return;
            }
        }
        if (index >= PlayerStats.playerCanHold) {
            index = PlayerStats.playerCanHold - 1; //not static
        }
        handPosition = index;
        foodInHand = true;
        SetSpecificOrderSpriteActive(getCustomerCount - 1, true);
        transform.localPosition = Vector3.zero;
    }

    public void DestroyFood() {
        Destroy(gameObject);
    }
    public bool GetCheckObject() {
        return checkObject;
    }
    
    private void Update() {
        transform.eulerAngles = Vector3.zero;
    }

    private void SetFoodSprite(Sprite newSprite, int index) {
        customerOrderSprite[getCustomerCount - 1].SetFoodSprite(newSprite, index);
    }
    private void SetTableIdSprite(Sprite newSprite) {
        tableIdSprite.sprite = newSprite;
    }

    private void SetAllOrderSpriteActive(bool status) {
        for (int i = 0; i < customerOrderSprite.Length; i++) {
            customerOrderSprite[i].SetOrderSpriteActive(status);
        }
    }
    private void SetSpecificOrderSpriteActive(int index, bool status) {
        customerOrderSprite[index].SetOrderSpriteActive(status);
    }


}
