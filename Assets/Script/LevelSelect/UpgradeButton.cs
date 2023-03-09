using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Upgrade {
    cookingTime, tableCount
}

public class UpgradeButton : MonoBehaviour {

    [SerializeField]
    Upgrade upgradeType;
    [SerializeField]
    int coins;
    [SerializeField]
    Text currentValue, coinValue;
    [SerializeField]
    Button button;
    [SerializeField]
    Slider slider;
    int startingCoins;
    bool full;

    void Start () {
        startingCoins = coins;
        button.interactable = false;
        checkCoins();
        updateContent();
    }
	
	void updateContent () {
        float sliderValue = 0;
        if (upgradeType == Upgrade.cookingTime) {
            currentValue.text = GameData.getCookingTime().ToString();
            sliderValue = GameData.getCookingTimePercent();
        } else if (upgradeType == Upgrade.tableCount) {
            currentValue.text = GameData.getTableCount().ToString();
            sliderValue = GameData.getTablePercent();
        }
        StartCoroutine(setSliderValue(sliderValue));
    }

    void UpdateAllButton() {
        UpgradeButton[] ups = FindObjectsOfType<UpgradeButton>();
        for(int i = 0; i < ups.Length; i++) {
            ups[i].checkCoins();
        }
    }

    public void checkCoins() {
        coins = Mathf.RoundToInt(startingCoins * (1 + slider.value));
        coinValue.text = coins.ToString();
        full = false;
        if (slider.value >= 1) {
            button.interactable = false;
            slider.value = 1;
            coinValue.text = "FULL";
            full = true;
        }
        if (!full && GameData.getCoin() >= coins) {
            button.interactable = true;
            print("Coins: " + coins);
        } else {
            button.interactable = false;
        }
    }

    IEnumerator setSliderValue(float value) {
        float timer = 0;
        float diff = value - slider.value;
        float startValue = slider.value;
        button.interactable = false;
        coinValue.text = "wait";
        while(timer < 1) {
            timer += Time.deltaTime;
            slider.value = startValue + timer * diff;
            yield return null;
        }
        slider.value = value;
        checkCoins();
    }

    public void upgrade() {
        if (upgradeType == Upgrade.cookingTime) {
            GameData.incrementCookingTime();
        } else if (upgradeType == Upgrade.tableCount) {
            GameData.incrementTable();
        }
        GameData.addCoin(-coins);
        CoinDisplay.instance.updateCoin();
        UpdateAllButton();
        updateContent();
    }
}
