using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour {

    [SerializeField]
    private FoodSpawnHandler foodSpawnHandler;

	public void InitializeData() {
        GameLevelManager.Instance.GetHUDManager().GetScoreManager().SetPlayerGoal(LevelManager.Instance.GetDataManager().GetLevelGoal());
        GameLevelManager.Instance.GetHUDManager().GetTimeManager().SetStartTime(LevelManager.Instance.GetDataManager().GetLevelTime());
        GameLevelManager.Instance.GetCustomerHandler().SetCustomerTimeDelayToSpawn(LevelManager.Instance.GetDataManager().GetCustomerRespawnTimer());

        foodSpawnHandler.SetFoodSpawnDelay(LevelManager.Instance.GetDataManager().GetCookingTime());

        GameLevelManager.Instance.GetTableSetManager().SetAllTableActive(false);
        for(int i = 0; i < LevelManager.Instance.GetDataManager().GetTableCount(); i++) {
            GameLevelManager.Instance.GetTableSetManager().SetEachTableActive(i, true);
        }
    }
}
