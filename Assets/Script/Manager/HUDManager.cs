using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {


    [SerializeField]
    private ScoreManager scoreManager = null;
    [SerializeField]
    private TimeManager timeManager = null;

    [SerializeField]
    Text levelDisplay, locationDisplay;

    private void Start() {
        //if (Instance != this)
        //    Instance = this;
        //else
        //    Destroy(gameObject);

        locationDisplay.text = GameData.activeStage.name;
        levelDisplay.text = "Level " + (GameData.chosenLevel + 1);
    }

    public ScoreManager GetScoreManager() {
        return scoreManager;
    }
    public TimeManager GetTimeManager() {
        return timeManager;
    }
}
