using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : InteractableObject {

    public enum TableState { AVAILABLE, OCCUPIED };
    private TableState tableState = TableState.AVAILABLE;

    public int tableId = 0;

    [SerializeField]
    private GameObject orderIdGO = null;

    [SerializeField]
    SpriteRenderer[] plates;

    [SerializeField]
    private SpriteRenderer tableNumberSR = null;

    [SerializeField]
    private CoinVFX tableCoinVFX = null;

    private CustomerInfoHandler currentCustomerIH = null;

    #region ForDebugging
    [SerializeField]
    private TextMesh tableStateTextMesh;
    private TableState newTableState = TableState.AVAILABLE;
    #endregion

    public override void Start() {
        base.Start();
        tableStateTextMesh.text = GetCurrentTableState().ToString();
        tableState = TableState.AVAILABLE;
        SetCheckActiveOff();
        LayerManager.instance.updateLayer(transform);
        setFoodSprite(null);
    }
    private void Update() {
        if (newTableState != GetCurrentTableState()) {
            tableStateTextMesh.text = GetCurrentTableState().ToString();
            newTableState = GetCurrentTableState();
        }
    }

    public void ChangeTableState(TableState newTableState) {
        tableState = newTableState;
    }
    public TableState GetCurrentTableState() {
        return tableState;
    }

    public void SetCurrentCustomer_InfoHandler(CustomerInfoHandler currentCustomerIH) {
        this.currentCustomerIH = currentCustomerIH;
    }
    public CustomerInfoHandler GetCurrentCustomer_InfoHandler() {
        if(currentCustomerIH == null) {
            Debug.Log("CurrentCustomer is null");
        }

        return currentCustomerIH;
    }

    public Sprite GetTableNumberSprite() {
        return tableNumberSR.sprite;
    }

    public void SetOrderID_Active(bool activateState) {
        orderIdGO.SetActive(activateState);
    }
    public void NotifyCustomerBatch_PlayerArrival() {
        if(currentCustomerIH == null) {
            return;
        }

        currentCustomerIH.PlayerVisited();
    }

    public void setFoodSprite(Sprite[] sprite, int count = 0) {
        if (sprite == null) {
            for (int i = 0; i < plates.Length; i++) {
                plates[i].enabled = false;
            }
        } else {
            for (int i = 0; i < plates.Length; i++) {
                if (i < count) {
                    plates[i].sprite = sprite[i];
                    plates[i].enabled = true;
                }else {
                    plates[i].enabled = false;
                }
            }
        }
    }

    public void TriggerSplashCoins() {
        tableCoinVFX.TriggerCoinVFX();
    }

    public void SetCoinValue(string paymentText) {
        tableCoinVFX.SetCoinValue(paymentText);
    }

}
