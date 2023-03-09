using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHandler : MonoBehaviour {

    [SerializeField]
    private List<CustomerWaitingSpot> customerWaitingSpots;

    [SerializeField]
    private CustomerBatch[] batchCustomers;

    [SerializeField]
    private Animator closeSign = null;

    List<CustomerBatch> listCustomerBatch;

    [SerializeField]
    private float timeDelayToSpawn;

    private Vector3 getScreentoWorldPosition = Vector3.zero;
    private Collider2D getCurrentCollider2D;
    private bool canSpawn = true;
    private bool cuisineClosed = false;

    private void Start () {
        //if (Instance == null)
        //    Instance = this;
        //else if (Instance != this)
        //    Destroy(gameObject);

        listCustomerBatch = new List<CustomerBatch>();
        SetCloseSignAnimation(cuisineClosed);
    }
    private void Update() {
        if (canSpawn && !cuisineClosed) {
            StartCoroutine(SpawnBatchOfCustomer());
        }
        if(cuisineClosed != true) {
            cuisineClosed = GameLevelManager.Instance.GetGameStateManager().GetCurrentGameState() == GameState.CLOSE;
            SetCloseSignAnimation(cuisineClosed);
        }
    }

    private bool SpotAvailable() {
        foreach(CustomerWaitingSpot cws in customerWaitingSpots) 
        {
            if (!cws.isSpotTaken) {
                return true;
            }
        }
        return false;
    }
    public bool IsCustomerLineCleared() {
        foreach (CustomerWaitingSpot cws in customerWaitingSpots) {
            if (cws.isSpotTaken) {
                return false;
            }
        }
        return true;
    }

    private IEnumerator SpawnBatchOfCustomer() {
        canSpawn = false;
        while (SpotAvailable()) {
            yield return new WaitForSeconds(timeDelayToSpawn);
            if (cuisineClosed) {
                yield break;
            }
            Vector3 getAvailablePosition = Vector3.zero;
            int getWaitingSpotID = 0;
            for(int i = 0; i < customerWaitingSpots.Count; i++) {
                if (!customerWaitingSpots[i].isSpotTaken) {
                    getAvailablePosition = customerWaitingSpots[i].spotTransfom.position;
                    customerWaitingSpots[i].isSpotTaken = true;
                    getWaitingSpotID = i;
                    break;
                }
            }

            CustomerBatch customerBatch = Instantiate(batchCustomers[Random.Range(0, batchCustomers.Length)], getAvailablePosition, Quaternion.identity);
            listCustomerBatch.Add(customerBatch);
            customerBatch.WaitingSpotID = getWaitingSpotID;
        }
        canSpawn = true;
    }
    public void RemoveCustomerBatchQueue(int index) {
        listCustomerBatch.Remove(listCustomerBatch[index]);
    }
    public void BatchCustomer_OnWaitingAreaUpdate() { //customer waiting area must update > empty position between customer spots must be filled in
        SetAllWaitingSpotStatus(false);
        for (int i = 0; i < listCustomerBatch.Count; i++) {
             listCustomerBatch[i].transform.position = customerWaitingSpots[i].spotTransfom.position;
             customerWaitingSpots[i].isSpotTaken = true;
             listCustomerBatch[i].WaitingSpotID = i;
        }
    }

    public void CustomerBatch_OnDrag(CustomerBatch customerBatch) {
        getScreentoWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        getScreentoWorldPosition.z = transform.position.z;
        customerBatch.SetCustomerBatchParent_Position(getScreentoWorldPosition, false);
        customerBatch.UpdateLayer(true);
        
    } //While customerBatch is currently dragging
    public void CustomerBatch_OnMouseUp(CustomerBatch customerBatch) {
        if (customerBatch.tableRange) {
            SetCustomerOnTable(customerBatch);
        } else {
            SetCustomerOnWaitingSpot(customerBatch);
        }
        customerBatch.UpdateLayer();
    } //While customerBatch is from drag to OnMouseUp

    public void CustomerBatch_OnTableRange(CustomerBatch customerBatch, Collider2D col) {
       customerBatch.SetBatchCustomer_Parent(col.transform, Vector3.zero, true);
       customerBatch.SetBatchCustomer_Position(Vector3.zero, true);
       customerBatch.SetAllCustomerTo_LocalPositionWhenSeated();
       customerBatch.SetPatienceMeterTo_PositionWhenSeated();
       getCurrentCollider2D = col;
    } //When customerBatch is on table range (run only once)
    public void CustomerBatch_NotOnTableRange(CustomerBatch customerBatch, Collider2D col) {
        customerBatch.SetBatchCustomer_Parent(customerBatch.transform, Vector3.zero, true);
        customerBatch.SetAllCustomerTo_OriginalPosition();
        customerBatch.SetPatienceMeterTo_OriginalPosition();

    } //When customerBatch is off table range from table range (run only once)

    public void SetCustomerSpot_IsSpotTaken(int waitingSpotID, bool available) {
        customerWaitingSpots[waitingSpotID].isSpotTaken = available;
    }
    public void SetCustomerTimeDelayToSpawn(float timeDelayToSpawn) {
        this.timeDelayToSpawn = timeDelayToSpawn;
    }

    private void SetCustomerOnTable(CustomerBatch customerBatch) {
        Table getTable = getCurrentCollider2D.GetComponent<Table>();
        if (getTable != null) {
            customerBatch.SetTableReference(getTable);
            getTable.ChangeTableState(Table.TableState.OCCUPIED);
            customerBatch.SetCustomerState(CustomerState.ORDERING);
            getTable.SetCurrentCustomer_InfoHandler(customerBatch.GetCustomerInfoHandler());
        }

        customerWaitingSpots[customerBatch.WaitingSpotID].isSpotTaken = false;
        RemoveCustomerBatchQueue(customerBatch.WaitingSpotID);
        BatchCustomer_OnWaitingAreaUpdate();

        customerBatch.SetBatchCustomer_Parent(customerBatch.transform, Vector3.zero, true);
        customerBatch.SetCustomerBatchParent_Parent(getCurrentCollider2D.transform, Vector3.zero, true);
        customerBatch.SetAllCustomerTo_LocalPositionWhenSeated();
        customerBatch.SetPatienceMeterTo_PositionWhenSeated();
        customerBatch.batchCustomerBoxCollider2D.enabled = false;

    }
    private void SetCustomerOnWaitingSpot(CustomerBatch customerBatch) {
        customerBatch.SetAllCustomerTo_OriginalPosition();
        customerBatch.SetPatienceMeterTo_OriginalPosition();
        customerBatch.SetCustomerBatchParent_Position(customerWaitingSpots[customerBatch.WaitingSpotID].spotTransfom.position, false);
    }

    private void SetAllWaitingSpotStatus(bool isSpotTaken) {
        for (int i = 0; i < customerWaitingSpots.Count; i++) {
            customerWaitingSpots[i].isSpotTaken = isSpotTaken;
        }
    }
    private void SetCloseSignAnimation(bool closed) {
        closeSign.SetBool("Closed", closed);
    }
}
