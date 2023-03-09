using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomerInfoHandler))]
public class CustomerBatch : MonoBehaviour {
    
    public Transform[] customerPositions;
    public Vector3[] customerLocalPositionWhenInTable;
    public Vector3 patienceMeterLocalPositionWhenInTable;
    public BoxCollider2D batchCustomerBoxCollider2D;

    [SerializeField]
    private GameObject batchCustomer; //the one who carries the customers / 1st child of this gameobject
    [SerializeField]
    private CustomerInfoHandler customerInfoHandler = null;

    public CustomerPatienceManager customerPatienceManager = null;

    private int waitingSpotID = 0;
    private bool getNewTableRange = false;
    private bool tableRangeCheck = true;

    private CustomerHandler customerHandler = null;
    private Vector3 originalPosition = Vector3.zero;
    private Vector3 patienceMeterOriginalPosition = Vector3.zero;
    private List<Vector3> customerOriginalPosition;
    private Vector3 originalScale = Vector3.zero;
    private Collider2D col2D;
    private Sprite[] foodSprite = null;
    private Sprite tableIDSprite = null;


    [HideInInspector]
    public bool tableRange = false;

    private void Start() {
        foodSprite = new Sprite[customerInfoHandler.customerInfo.customerCount];
        originalPosition = transform.position;
        patienceMeterOriginalPosition = customerPatienceManager.transform.localPosition;
        customerOriginalPosition = new List<Vector3>();

        for (int i = 0; i < customerPositions.Length; i++) {
            Vector3 getCustomerSpritePosition = customerPositions[i].transform.localPosition;
            customerOriginalPosition.Add(getCustomerSpritePosition);
            originalScale = customerPositions[i].localScale;
            LayerManager.instance.updateLayer(customerPositions[i]);
        }

        customerHandler = GameLevelManager.Instance.GetCustomerHandler();
        customerInfoHandler = GetComponent<CustomerInfoHandler>();
    }
    private void Update() {
        if (tableRangeCheck && getNewTableRange != tableRange) {
            if (tableRange) {
                customerHandler.CustomerBatch_OnTableRange(this, col2D);
            } else {
                customerHandler.CustomerBatch_NotOnTableRange(this, col2D);
            }
            getNewTableRange = tableRange;
        }
    }

    public int WaitingSpotID {
        get { return waitingSpotID; }
        set { waitingSpotID = value; }
    }

    /* Explanation of the Customer System
    *
    * Current System of Customers:
    *      - CustomerBatch is being controlled by this gameobject (OnMouseDrag & OnMouseUp)
    *      - CustomerHandle is the one who will decide whatever the customerBatch will do
    *      - CustomerBatch calls the CustomerBatchHandler whenever something happens (MouseDrag,MouseUp,Triggers(Enter/Exit))
    *      - CustomerBatch has the instruction on what will happen to it. Only the CustomerBatchHandler will decide when it will be called
    *       
    *      - The CustomerBatch(The one who has this script) is the one who has the box collider (Mouse movement purposes)
    *      - The CustomerBatchGameObject(The one who carries all the customer/child#1 of this gameobject) is the one who is being parented to the table
    *      - When the batchCustomer is being dragged and:
    *               - it reaches a table range:
    *                   - the CustBatchGO is being parented to the table
    *               - it doesnt reaches a table range (or leaves on it)
    *                   - the CustBatchGO is being parented to this object 
    *       
    *   - When the batchCustomer is being dragged AND the player releases the Mouse (MouseUp) and:
    *               - it reaches a table range:
    *                   - it disables the Collider2D
    *                   - it sets the position and layer when seated
    *               - it doesnt reaches a table range (or leaves on it)
    *                   - it goes back to its position where it is being dragged
    *     
    */

    private void OnMouseDrag() {
        customerHandler.CustomerBatch_OnDrag(this);
    }
    private void OnMouseUp() {
        if (tableRange) {
            tableRangeCheck = false;
        }
        customerHandler.CustomerBatch_OnMouseUp(this);
    }

    public void UpdateLayer(bool drag = false) {
        for (int i = 0; i < customerPositions.Length; i++) {
            LayerManager.instance.updateLayer(customerPositions[i]);
            if (drag) {
                customerPositions[i].localPosition -= Vector3.forward * 2;
            }
        }
    }

    public void SetAllCustomerTo_OriginalPosition() {
        for (int i = 0; i < customerPositions.Length; i++) {
            customerPositions[i].transform.localPosition = customerOriginalPosition[i];
            customerPositions[i].transform.localScale = originalScale;
        }
    }
    public void SetAllCustomerTo_LocalPositionWhenSeated() {
        for (int i = 0; i < customerPositions.Length; i++) {
            customerPositions[i].transform.localPosition = customerLocalPositionWhenInTable[i];
            if (i % 2 != 0) {
                customerPositions[i].transform.localScale = new Vector3(originalScale.x * -1, originalScale.y, originalScale.z);
            }
        }
    }

    public void SetCustomerBatchParent_Position(Vector3 newPosition, bool changeLocalPosition) {
        if (changeLocalPosition) {
            transform.localPosition = newPosition;
            return;
        }
        transform.position = newPosition;
    }
    public void SetCustomerBatchParent_Parent(Transform newParent, Vector3 newLocalPosition, bool willChangeLocalPosition) {
        transform.parent = newParent;
        if (willChangeLocalPosition) {
            transform.localPosition = newLocalPosition;
        }
    }

    public void SetBatchCustomer_Position(Vector3 newPosition, bool changeLocalPosition) {
        if (changeLocalPosition) {
            batchCustomer.transform.localPosition = newPosition;
            return;
        }
        batchCustomer.transform.position = newPosition;
    }
    public void SetBatchCustomer_Parent(Transform newParent, Vector3 newLocalPosition, bool willChangeLocalPosition) {
        batchCustomer.transform.parent = newParent;
        if (willChangeLocalPosition) {
            batchCustomer.transform.localPosition = newLocalPosition;
        }
    } //this uses the child of the batchCustomer as the holder for the customer

    public void SetTableReference(Table getTableReference) {
        customerInfoHandler.customerInfo.SetCustomerCurrentTable(getTableReference);
    }
    public void SetCustomerState(CustomerState newState) {
        customerInfoHandler.customerInfo.SetCustomerCurrentState(newState);
        customerInfoHandler.SetAnimationState((int)newState);
    }

    public void SetPatienceMeterTo_OriginalPosition() {
        customerPatienceManager.transform.parent = customerInfoHandler.transform;
        customerPatienceManager.transform.localPosition = patienceMeterOriginalPosition;
    }
    public void SetPatienceMeterTo_PositionWhenSeated() {
        customerPatienceManager.transform.parent = customerInfoHandler.customerInfo.transform;
        customerPatienceManager.transform.localPosition = patienceMeterLocalPositionWhenInTable;
    }

    public CustomerInfoHandler GetCustomerInfoHandler() {
        return customerInfoHandler;
    }
    public void DestroyCustomerBatch(float delay = 0f) {
        if(delay < 0) {
            delay = 0;
        }
        if (customerInfoHandler.customerInfo.GetCustomerCurrentState() == CustomerState.WAITING_IN_LINE) {
            customerHandler.RemoveCustomerBatchQueue(WaitingSpotID);
            customerHandler.BatchCustomer_OnWaitingAreaUpdate();
        }
        Destroy(gameObject, delay);
    }

    public void SetCustomerOrderSprite(Sprite foodSprite, int index) {
        this.foodSprite[index] = foodSprite;
    }
    public Sprite GetCustomerOrderSprite(int index) {
        return foodSprite[index];
    }
    public Sprite[] GetAllCustomerOrderSprite() {
        return foodSprite;
    }

    public void SetCustomerTableIdSprite(Sprite tableIDSprite) {
        this.tableIDSprite = tableIDSprite;
    }
    public Sprite GetCustomerTableIdSprite() {
        return tableIDSprite;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer != LayerMask.NameToLayer("Table")) {
            return;
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("Table")) {
            if (col.gameObject.GetComponent<Table>().GetCurrentTableState() == Table.TableState.OCCUPIED) {
                return;
            }
        }
        customerInfoHandler.SetAnimationState(1);
        col2D = col;
        tableRange = true;
    }
    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.layer != LayerMask.NameToLayer("Table")) {
            return;
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("Table")) {
            if (col.gameObject.GetComponent<Table>().GetCurrentTableState() == Table.TableState.OCCUPIED) {
                return;
            }
        }

        col2D = col;
        customerInfoHandler.SetAnimationState(0);
        tableRange = false;
    }
}
