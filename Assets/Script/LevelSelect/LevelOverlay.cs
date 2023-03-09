using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOverlay : MonoBehaviour {

    [SerializeField]
    Slider progress;
    [SerializeField]
    Button[] level;
    [SerializeField]
    Sprite done,current,locked;
    [SerializeField]
    GameObject minigame;

    public void OnEnable() {
        StopAllCoroutines();
        StartCoroutine(showProgress());
        for (int i = 0; i < level.Length; i++) {
            level[i].interactable = i <= GameData.activeStage.getMaxLevelUnlocked();
        }
        if(GameData.activeStage.getMaxLevelUnlocked() > 2) {
            minigame.SetActive(true);
        }else {
            minigame.SetActive(false);
        }
    }

    public void changeChangeButtonsSprite() {
        for (int i = 0; i < level.Length; i++) {
            if (i == GameData.chosenLevel) {
                level[i].GetComponent<Image>().sprite = current;
            }else if (i <= GameData.activeStage.getMaxLevelUnlocked()) {
                level[i].GetComponent<Image>().sprite = done;
            }else {
                level[i].GetComponent<Image>().sprite = locked;
            }
        }
    }

    IEnumerator showProgress() {
        yield return new WaitForSecondsRealtime(1);
        float timer = 0;
        float percent = (float)GameData.activeStage.getMaxLevelUnlocked() / 2f;
        while (timer < 1) {
            progress.value = timer * percent;
            timer += Time.deltaTime;
            yield return null;
        }
    }

}
