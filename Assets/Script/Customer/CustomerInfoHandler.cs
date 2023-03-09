using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInfoHandler : MonoBehaviour {

    [SerializeField]
    private GameObject orderNoticeGO = null;

    [SerializeField]
    private CustomerOrderSprite customerOrderSprite = null;

    public CustomerInfo customerInfo;

    [SerializeField]
    private Note note = null;

    [SerializeField]
    private CustomerTypeStats[] customerTypes;

    [SerializeField]
    private bool debugMode = false; //customer will go to each state every x seconds

    private CustomerBatch customerBatch = null;
    private Note noteReference = null;
    private Food foodReference = null;
    private PlayerController playerController = null;

    private List<int> getRandomRecord = new List<int>();
    
    private bool customerStartedOrdering = false;
    private bool checkingPatience = true;
    private float customerOriginalPatienceValue = 0;
    private float customerFadingPatienceValue = 0;
    private float customerIncrementPatienceValue = 0;

    [Header("Dbggng. Setting of stats is in the prefab of each customer type")]
    [SerializeField]
    private float customerPayment = 1f;
    [Header("Dbggng. Setting of stats is in the prefab of each customer type")]
    [SerializeField]
    private float customerEatingSpeed = 1f;
    [Header("Dbggng. Setting of stats is in the prefab of each customer type")]
    [SerializeField]
    private float customerOrderingSpeed = 1f;

	void Awake () {
        Initialize();
    }
    private void Initialize() {
        customerBatch = GetComponent<CustomerBatch>();
        playerController = GameLevelManager.Instance.GetPlayerController();
        int i = 0;
        foreach (Animator a in customerInfo.customerAnimators) {

            again:
            int getRandomCustomerType = Random.Range(0, customerTypes.Length);
            if (getRandomRecord.Contains(getRandomCustomerType)) {
                goto again;
            }
            getRandomRecord.Add(getRandomCustomerType);
            customerPayment += customerTypes[getRandomCustomerType].payment;
            customerOrderingSpeed = customerTypes[getRandomCustomerType].orderingSpeed;
            customerEatingSpeed = customerTypes[getRandomCustomerType].eatingSpeed;
            customerInfo.patienceValue = customerTypes[getRandomCustomerType].patienceValue;
            customerInfo.hearts = customerTypes[getRandomCustomerType].startingHeartPiece;

            customerInfo.hearts = (customerInfo.hearts > 3 ? 3 : (customerInfo.hearts < 0 ? 0 : customerInfo.hearts));

            customerBatch.customerPatienceManager.SetActivePatience(customerTypes[getRandomCustomerType].startingHeartPiece);
            customerOriginalPatienceValue = customerTypes[getRandomCustomerType].patienceValue;
            customerFadingPatienceValue = customerTypes[getRandomCustomerType].fadingPatienceValue;
            customerIncrementPatienceValue = customerTypes[getRandomCustomerType].incrementPatienceValue;
            a.runtimeAnimatorController = customerTypes[getRandomCustomerType].GetComponent<Animator>().runtimeAnimatorController;
            PartsSprite sprite = customerTypes[getRandomCustomerType].spriteSheet[GameData.activeStage.index];

            customerInfo.setSprites(i++, sprite.sprite[0], sprite.sprite[1], sprite.sprite[2], sprite.sprite[3], sprite.sprite[4], sprite.sprite[5], sprite.sprite[6], sprite.sprite[7], sprite.sprite[8]);
            
        }

        StartCoroutine(CustomerPatience());
    }

    void Update() {
        if(!customerStartedOrdering && customerInfo.GetCustomerCurrentState() == CustomerState.ORDERING) {
            StartCoroutine(CustomerOrdering());
            customerStartedOrdering = true;
        }
    } 

    private IEnumerator CustomerOrdering() {
        CustomerOrderState();

        yield return new WaitForSecondsRealtime(customerOrderingSpeed);

        StopCoroutine(CustomerOrdering());
        StartCoroutine(CustomerOrderReady());
    } //goes here when customer is ordering
    private void CustomerOrderState() {
        SetCheckPatienceActive(false);
        UpdateCustomerPatienceValue(customerIncrementPatienceValue);
    }

    private IEnumerator CustomerOrderReady() {
        CustomerOrderReadyState();
        yield return new WaitForEndOfFrame();
        StopCoroutine(CustomerOrderReady());
        if (debugMode) {
            StartCoroutine(CustomerWaitingForFood());
        }
    } //goes here when customer ready for his/her order
    private void CustomerOrderReadyState() {
        SetCheckPatienceActive(true);
        customerInfo.SetCustomerCurrentState(CustomerState.ORDER_READY);
        SetAnimationState((int)customerInfo.GetCustomerCurrentState());
        SetOrderNoticeActive(true);
        SetCustomerOrderSpriteActive(true);

        FoodData[] data = new FoodData[customerInfo.customerCount];
        for(int i = 0; i < customerInfo.customerCount; i++) {
            data[i] = LevelManager.Instance.GetFoodManager().PickFood();
            customerBatch.SetCustomerOrderSprite(data[i].sprite, i);
            customerPayment += data[i].getPrice();
            SetCustomerFoodSprite(customerBatch.GetCustomerOrderSprite(i), i);
        }

        customerBatch.SetCustomerTableIdSprite(customerInfo.GetCustomerCurrentTable().GetTableNumberSprite());
    }

    private IEnumerator CustomerWaitingForFood() {
        CustomerWaitingForFoodState();

        yield return new WaitForEndOfFrame();

        StopCoroutine(CustomerWaitingForFood());
        if (debugMode) {
            StartCoroutine(CustomerEatingFood());
        }
    } //goes here when customer still waiting for his/her order
    private void CustomerWaitingForFoodState() {
        UpdateCustomerPatienceValue(customerIncrementPatienceValue);
        noteReference = Instantiate(note, transform.position, Quaternion.identity);
        noteReference.SetOrder(customerInfo.GetCustomerCurrentTable().tableId, customerBatch);
        int setPlaceOfNote = playerController.playerInfo.GetCarryCount(); //if player has no carry, it will go to 1st position. else, 2nd position

        noteReference.SetLocalPosition_WhenInPlayer(setPlaceOfNote, playerController.playerMovement.arm);
        playerController.playerInfo.SetCarryCount(++setPlaceOfNote);
        playerController.playerInfo.SetNote(noteReference, setPlaceOfNote - 1);
        customerInfo.GetCustomerCurrentTable().SetOrderID_Active(true);

        customerInfo.SetCustomerCurrentState(CustomerState.WAITING_FOR_FOOD);

        SetCustomerOrderSpriteActive(false);
        SetOrderNoticeActive(false);

        if (playerController.playerInfo.GetCarryCount() == 1) {
            playerController.playerInfo.SetPlayerState(PlayerState.HOLDING_ORDER, "CustomerInfoHandler.cs");
        } else {
            if (playerController.playerInfo.GetPlayerState() == PlayerState.HOLDING_FOOD) {
                playerController.playerInfo.SetPlayerState(PlayerState.HOLDING_FOOD_ORDER, "CustomerInfoHandler.cs");
                return;
            }
            playerController.playerInfo.SetPlayerState(PlayerState.HOLDING_ORDER_2, "CustomerInfoHandler.cs");
        }
    }

    private IEnumerator CustomerEatingFood() {
        CustomerEatingFoodState();
        yield return new WaitForSecondsRealtime(customerEatingSpeed);
        customerInfo.SetCustomerCurrentState(CustomerState.PAYING);
        SetAnimationState((int)customerInfo.GetCustomerCurrentState());
        SetCheckPatienceActive(true);

        if (debugMode) {
            StartCoroutine(CustomerPayFood());
        }
    } //goes here when customer still eating
    private void CustomerEatingFoodState() {
        SetCustomerOrderSpriteActive(false);
        SetCheckPatienceActive(false);
        UpdateCustomerPatienceValue(customerIncrementPatienceValue);
        customerInfo.GetCustomerCurrentTable().setFoodSprite(customerBatch.GetAllCustomerOrderSprite(), customerInfo.customerCount);

        customerInfo.GetCustomerCurrentTable().SetOrderID_Active(false);
        customerInfo.SetCustomerCurrentState(CustomerState.EATING);
    }

    private IEnumerator CustomerPayFood() {
        CustomerPayFoodState();
        yield return new WaitForEndOfFrame();
    } //goes here when customer is prepared to pay
    private void CustomerPayFoodState() {
        customerInfo.GetCustomerCurrentTable().setFoodSprite(null);
        int getCustomerHearts = customerInfo.hearts;
        for (int i = 0; i < 3 - getCustomerHearts; i++) {
            customerPayment *= 0.5f;
            customerPayment = Mathf.Ceil(customerPayment);
        }
        GameLevelManager.Instance.GetHUDManager().GetScoreManager().AddPlayerScore(customerPayment);
        customerInfo.GetCustomerCurrentTable().SetCoinValue(customerPayment.ToString());
        customerInfo.GetCustomerCurrentTable().TriggerSplashCoins();
        AudioManager.instance.coin.Play();
        customerInfo.GetCustomerCurrentTable().SetOrderID_Active(false);
        customerInfo.GetCustomerCurrentTable().ChangeTableState(Table.TableState.AVAILABLE);
        DestroyCustomer();
    }

    private IEnumerator CustomerPatience() {
        bool customerStay = true;
        while (customerStay) {
            yield return new WaitForSecondsRealtime(1f);
            if (checkingPatience) {
                customerInfo.patienceValue--;
                if(customerInfo.patienceValue <= customerFadingPatienceValue && customerBatch.customerPatienceManager.GetPatienceInfo(customerInfo.hearts - 1).patienceState != PatienceState.FADING) {
                    ChangePatienceState(customerInfo.patienceValue);
                }
                if (customerInfo.patienceValue <= 0 && customerBatch.customerPatienceManager.GetPatienceInfo(customerInfo.hearts - 1).patienceState != PatienceState.EMPTY) {
                    ChangePatienceState(customerInfo.patienceValue);
                    customerInfo.patienceValue = customerOriginalPatienceValue;
                    customerInfo.hearts--;
                    SetAngryAnimation(customerInfo.hearts <= 1);
                }
                customerStay = customerBatch.customerPatienceManager.IsAllHeartNotEmpty();
            }
        }
        if (customerInfo.hearts <= 0) {
            checkingPatience = false;
        }
    }
    private bool ChangePatienceState(float patienceValue) {

        SetAngryAnimation(customerInfo.hearts <= 1);
        if (customerInfo.patienceValue <= 0) {
            customerBatch.customerPatienceManager.ChangePatienceInfoState(customerInfo.hearts - 1, PatienceState.EMPTY);
            return true;
        }
        else if (customerInfo.patienceValue <= customerFadingPatienceValue) {
            customerBatch.customerPatienceManager.ChangePatienceInfoState(customerInfo.hearts - 1, PatienceState.FADING);
            return true;
        }
        else if (customerInfo.patienceValue > customerFadingPatienceValue) {
            customerBatch.customerPatienceManager.ChangePatienceInfoState(customerInfo.hearts - 1, PatienceState.FULL);
            return true;
        }
        return false;
    }
    private void UpdateCustomerPatienceValue(float addValue) {
        if(customerInfo.hearts <= 0) {
            return;
        }
        customerInfo.patienceValue += addValue;
        int getCustomerHeartCount = customerBatch.customerPatienceManager.GetPatienceInfoCount();

        if (customerInfo.patienceValue >= customerOriginalPatienceValue) {
            ChangePatienceState(customerInfo.patienceValue);
            if (customerInfo.hearts < getCustomerHeartCount) {
                float getDifference = Mathf.Abs(customerInfo.patienceValue - customerOriginalPatienceValue);
                customerInfo.hearts++;
                if (customerInfo.hearts > getCustomerHeartCount) {
                    customerInfo.hearts = getCustomerHeartCount;
                }
                customerInfo.patienceValue = getDifference;
                customerBatch.customerPatienceManager.SetActivePatience(customerInfo.hearts);
            } else {
                customerInfo.patienceValue = customerOriginalPatienceValue;
            }
        }
        ChangePatienceState(customerInfo.patienceValue);
    }

    public void PlayerVisited() {
        if (customerInfo.GetCustomerCurrentState() == CustomerState.ORDER_READY) {
            StartCoroutine(CustomerWaitingForFood());
        } else if (customerInfo.GetCustomerCurrentState() == CustomerState.WAITING_FOR_FOOD) {
            if (playerController.playerInfo.IsPlayerHoldingFood()) {
                StartCoroutine(CustomerEatingFood());
            }
        } else if (customerInfo.GetCustomerCurrentState() == CustomerState.PAYING) {
            StartCoroutine(CustomerPayFood());
        }
        SetAnimationState((int)customerInfo.GetCustomerCurrentState());
    }
    private void DestroyCustomer(float delay = 0f) {
        bool customerWaitingInLine = customerInfo.GetCustomerCurrentState() == CustomerState.WAITING_IN_LINE; //When customer goes away while waiting in line > make spot avail. Otherwise, don't make it avail
        if (customerWaitingInLine) {
            GameLevelManager.Instance.GetCustomerHandler().SetCustomerSpot_IsSpotTaken(customerBatch.WaitingSpotID, false);
        } else {
            customerInfo.GetCustomerCurrentTable().ChangeTableState(Table.TableState.AVAILABLE);
        }

        int getCarryCount = playerController.playerInfo.GetCarryCount();
        if(noteReference != null) {
            if (playerController.playerInfo.GetNote(noteReference.handPosition) != null) {
                playerController.playerInfo.GetNote(noteReference.handPosition).DestroyNote();
                playerController.playerInfo.SetCarryCount(--getCarryCount);
            }
        }

        if (foodReference != null) {
            if (playerController.playerInfo.GetFood(foodReference.handPosition) != null) {
                playerController.playerInfo.GetFood(foodReference.handPosition).DestroyFood();
                playerController.playerInfo.SetCarryCount(--getCarryCount);
            } else if (!foodReference.foodInHand) {
                foodReference.DestroyFood();
            }
        }


        if (customerInfo.GetCustomerCurrentTable() != null) {
            customerInfo.GetCustomerCurrentTable().SetOrderID_Active(false);
            customerInfo.GetCustomerCurrentTable().setFoodSprite(null);
        }

        customerBatch.DestroyCustomerBatch(delay);
    }
    private void SetOrderNoticeActive(bool active) {
        orderNoticeGO.SetActive(active);
    }
    private void SetCustomerOrderSpriteActive(bool active) {
        customerOrderSprite.SetOrderSpriteActive(active);
    }

    public void SetAnimationState(int state) {
        if(state == 4) {
            AudioManager.instance.eating.Play();
        }
        for (int i = 0; i < customerInfo.customerAnimators.Length; i++) {
            customerInfo.customerAnimators[i].SetInteger("state", state);
        }
    }
    public void SetAngryAnimation(bool angry) {
        if (angry) {
            AudioManager.instance.angry.Play();
        }
        for (int i = 0; i < customerInfo.customerAnimators.Length; i++) {
            customerInfo.customerAnimators[i].SetBool("angry", angry);
        }
    }

    private void SetCustomerFoodSprite(Sprite newSprite, int index) {
        customerOrderSprite.SetFoodSprite(newSprite, index);
    }

    public void SetCheckPatienceActive(bool active) {
        checkingPatience = active;
    }
    public void SetFoodReference(Food foodRefence) {
        this.foodReference = foodRefence;
    }
}
