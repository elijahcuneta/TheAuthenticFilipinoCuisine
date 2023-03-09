using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour {

    List<FoodData> foods;

    void Awake() {
        foods = new List<FoodData>();
    }

    public void Initialize(FoodData foodData) {
        if (Random.Range(0, 10) < 5) {
            foodData.Load();
            foods.Add(foodData);
        }
    }

    public FoodData PickFood() {
        if (foods.Count == 0) {
            FoodData[] temp = GameData.activeStage.food;
            foods.Add(temp[Random.Range(0, temp.Length)]);
        }
        int getRandomIndex = Random.Range(0, foods.Count);
        return foods[getRandomIndex];
    }

    public void UpgradeMastery() {
        for (int i = 0; i < foods.Count; i++) {
            foods[i].upgradeMastery();
        }
    }
}