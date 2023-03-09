using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {

    [SerializeField]
    TextMesh nameDisplay;
    [SerializeField]
    StageData data;

    SpriteRenderer display;

    Collider2D collider;

    [SerializeField]
    Sprite current_Sprite, done_Sprite, notReached_Sprite;

    public static Stage current;

    [SerializeField]
    SpriteRenderer[] dots;

    void Awake() {
        display = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        data.Load();
        nameDisplay.text = data.name;
        if (GameData.getMaxStageReached() == data.index) {
            if (current != this && data.getMaxLevelUnlocked() == 0) {
                nameDisplay.text = "";
                for (int i = 0; i < dots.Length; i++) {
                    dots[i].enabled = false;
                }
                collider.enabled = false;
                display.sprite = notReached_Sprite;
                StartCoroutine(spawnDots(true));
            }else {
                StartCoroutine(spawnDots(false));
            }
            current = this;
        } else if (GameData.getMaxStageReached() > data.index) {
            display.sprite = done_Sprite;
            for (int i = 0; i < dots.Length; i++) {
                dots[i].color = new Color(0, 0, 0, 0.5f);
            }
        } else {
            display.sprite = notReached_Sprite;
            collider.enabled = false;
            for (int i = 0; i < dots.Length; i++) {
                dots[i].enabled = false;
            }
        }
    }

    IEnumerator spawnDots(bool first) {
        if (dots.Length > 0) {
            float delay = Mathf.Clamp(3 / dots.Length, 0.5f, 2f);
            if (!first) { delay = 0; }
            for (int i = 0; i < dots.Length; i++) {
                dots[i].enabled = true;
                dots[i].color = Color.white;
                yield return new WaitForSecondsRealtime(delay);
            }
        }
        display.sprite = current_Sprite;
        collider.enabled = true;
        nameDisplay.text = data.name;
        GetComponent<Animator>().SetBool("current", true);
    }

    private void OnMouseDown() {
        if (LevelSelectOverlayManager.instance.enableStageButtonInput) {
            LevelSelectOverlayManager.instance.setStage(data);
        }
    }
}
