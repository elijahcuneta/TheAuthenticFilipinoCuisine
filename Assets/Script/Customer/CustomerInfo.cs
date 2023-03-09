using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomerState {
    WAITING_IN_LINE = 0,
    ORDERING = 1,
    ORDER_READY = 2,
    WAITING_FOR_FOOD = 3,
    EATING = 4,
    PAYING = 5
}

public class CustomerInfo : MonoBehaviour {

    //debugging
    [SerializeField]
    TextMesh textMesh = null;

    [SerializeField]
    TextMesh textIdMesh = null;
    CustomerBatch debugCustomerBatch;

    public Animator[] customerAnimators;
    public int customerCount = 2;

    [HideInInspector]
    public float patienceValue = 0;
    [HideInInspector]
    public int hearts = 3;

    CustomerSpriteManager[] spriteManager;
    //debug
    private CustomerState newCustomerState = CustomerState.WAITING_IN_LINE;
    private int newCustomerId = 0;

    private CustomerState customerState = CustomerState.WAITING_IN_LINE;
    public void SetCustomerCurrentState(CustomerState newCustomerState) {
        customerState = newCustomerState;
    }
    public CustomerState GetCustomerCurrentState() {
        return customerState;
    }

    private Table customerCurrentTable = null;
    public Table GetCustomerCurrentTable() {
        return customerCurrentTable;
    }
    public void SetCustomerCurrentTable(Table newTable) {
        customerCurrentTable = newTable;
    }

    private void Start() {
      textMesh.text = GetCustomerCurrentState().ToString();
      debugCustomerBatch = transform.parent.GetComponent<CustomerBatch>();
    }

    private void Update() {
        if (newCustomerState != GetCustomerCurrentState()) {
            textMesh.text = GetCustomerCurrentState().ToString();
            newCustomerState = GetCustomerCurrentState();
        }
        if(newCustomerId != debugCustomerBatch.WaitingSpotID) {
            textIdMesh.text = debugCustomerBatch.WaitingSpotID.ToString();
            newCustomerId = debugCustomerBatch.WaitingSpotID;
        }
    }

    public void setSprites(int index, Sprite arm_sprite, Sprite body_sprite, Sprite fork_sprite, Sprite head_sprite, Sprite angry_sprite, Sprite openMouth_sprite, Sprite legSeated_sprite, Sprite legStand_sprite, Sprite spoon_sprite) {
        if(spriteManager == null) {
            spriteManager = GetComponentsInChildren<CustomerSpriteManager>();
        }
        spriteManager[index].arm_sprite = arm_sprite;
        spriteManager[index].spoon_sprite = spoon_sprite;
        spriteManager[index].fork_sprite = fork_sprite;
        spriteManager[index].legSeated_sprite = legSeated_sprite;
        spriteManager[index].legStand_sprite = legStand_sprite;
        spriteManager[index].body_sprite = body_sprite;
        spriteManager[index].angry_sprite = angry_sprite;
        spriteManager[index].openMouth_sprite = openMouth_sprite;
        spriteManager[index].head_sprite = head_sprite;
    }

}
