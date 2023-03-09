using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour {

    public static CoinDisplay instance;

    Text display;

	void Awake () {
        instance = this;
        display = GetComponent<Text>();
        updateCoin();
    }
	
	public void updateCoin() {
        display.text = GameData.getCoin().ToString();
    }
}
