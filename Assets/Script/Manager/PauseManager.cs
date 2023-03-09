using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    [SerializeField]
    private GameObject pausePanel = null;

    [SerializeField]
    private SceneChanger sceneChanger = null;

	public void OnClickPause() {
        SetTimeScaleValue(0);
        pausePanel.SetActive(true);
    }

    public void OnResumeClicked() {
        SetTimeScaleValue(1);
    }

    public void OnRetryClicked(string sceneName) {
        SetTimeScaleValue(1);
        sceneChanger.SceneChange(sceneName);
    }

    public void OnQuitClicked(string sceneName) {
        SetTimeScaleValue(1);
        sceneChanger.SceneChange(sceneName);
    }

    private void SetTimeScaleValue(float time) {
        if (time > 1)
            time = 1;
        else if (time < 0)
            time = 0;

        Time.timeScale = time;
    }


}
