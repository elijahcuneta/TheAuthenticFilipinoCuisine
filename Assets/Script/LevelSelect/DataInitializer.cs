using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInitializer : MonoBehaviour {

    LevelManager levelManager;

    private void Start() {
        levelManager = LevelManager.Instance;
    }

    public void OnClickPlay() {
        InitializeAssets();
        InitializeFood();
        InitializeData();
    }

    private void InitializeAssets() {
        levelManager.GetAssetManager().SetSpriteLocation(GameData.activeStage.GetBackground());
        levelManager.GetAssetManager().SetSpriteFloor(GameData.activeStage.GetFloor());
    }
    private void InitializeFood() {
        for (int i = 0; i < GameData.activeStage.food.Length; i++) {
            levelManager.GetFoodManager().Initialize(GameData.activeStage.GetFood(i));
        }
    }
    private void InitializeData() {
        int getCurrentLevel = GameData.activeStage.getMaxLevelUnlocked();
        levelManager.GetDataManager().SetLevelGoal(GameData.activeStage.goal[getCurrentLevel]);
        levelManager.GetDataManager().SetLevelTime(GameData.activeStage.timer[getCurrentLevel]);
        levelManager.GetDataManager().SetTableCount(GameData.getTableCount());
        levelManager.GetDataManager().SetCookingTime(GameData.getCookingTime());
        levelManager.GetDataManager().SetCustomerRespawnTimer(GameData.activeStage.customerRespawnTimer[getCurrentLevel]);
    }

}
