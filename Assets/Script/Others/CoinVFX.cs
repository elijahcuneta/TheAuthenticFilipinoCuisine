using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinVFX : MonoBehaviour {

    [SerializeField]
    private Text coinValue = null;

    [SerializeField]
    private Animator coinAnimator = null;

    public void SetCoinValue(string valueText) {
        coinValue.text = valueText;
    }

    public void TriggerCoinVFX() {
        coinAnimator.SetTrigger("SplashCoin");
    }
}

