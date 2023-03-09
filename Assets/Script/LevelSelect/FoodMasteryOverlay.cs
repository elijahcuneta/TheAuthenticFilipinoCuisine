using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct FoodUIGroup {
    public Image img;
    public Text nameDisplay;
    public Slider masteryDisplay;
}

public class FoodMasteryOverlay : MonoBehaviour {

    [SerializeField]
    FoodUIGroup[] ui;

    public void OnEnable() {
        for(int i = 0; i < ui.Length; i++) {
            FoodData data = GameData.activeStage.food[i];
            ui[i].img.sprite = data.sprite;
            ui[i].nameDisplay.text = data.nameID;
            StartCoroutine(sliderTopUp(ui[i].masteryDisplay, (float)data.getMastery() / (float)FoodData.MAX));
        }
    }

    IEnumerator sliderTopUp(Slider slider, float targetValue) {
        yield return new WaitForSecondsRealtime(0.5f);
        float timer = 0;
        while(timer < 1) {
            slider.value = targetValue * timer;
            timer += Time.deltaTime;
            yield return null;
        }
    }
    
}
