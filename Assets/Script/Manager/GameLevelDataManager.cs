using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelDataManager : MonoBehaviour {

    [SerializeField]
    private LevelAssetsManager levelAssetsManager = null;
    [SerializeField]
    private LevelDataManager levelDataManager = null;

    [SerializeField]
    private GameObject OnBoarding = null;

    private void Start() {
        levelAssetsManager.InitializeAssets();
        levelDataManager.InitializeData();

        if(OnBoarding != null) {
            if (PlayerPrefs.GetInt("stage", 0) == 0 && PlayerPrefs.GetString("OnBoardingDone", "notDone") == "notDone") {
                OnBoarding.SetActive(true);
                PlayerPrefs.SetString("OnBoardingDone", "hooray");
            }
        }
    }
}
