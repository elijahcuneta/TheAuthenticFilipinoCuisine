using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    [SerializeField]
    private Slider levelTime;

    private float startTime = 60;
    private float currentTime;
    private bool timePaused = false;
    
    private void Start() {
        currentTime = GetStartTime();
        StartCoroutine(TickTime());
    }

    private IEnumerator TickTime() {
        while (currentTime > 0) {
            currentTime -= Time.deltaTime;
            levelTime.value = currentTime / startTime;
            yield return null;
        }
        levelTime.value = 0;
        TimesUp();
    }

    private void TimesUp() {
        GameLevelManager.Instance.GetGameStateManager().SetCurrentGameState(GameState.CLOSE);
    }
    public void SetStartTime(int startTime) {
        this.startTime = startTime;
    }
    public float GetStartTime() {
        return startTime;
    }

    public float GetCurrentTime() {
        return currentTime;
    }

    public void TimePause(bool active) {
        timePaused = active;
    }
}
