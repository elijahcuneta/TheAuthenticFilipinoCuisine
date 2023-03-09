using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectOverlayManager : MonoBehaviour {

    public static LevelSelectOverlayManager instance;
    [SerializeField]
    LevelOverlay lvlOverlay;
    [SerializeField]
    Text stageName;
    [SerializeField]
    FoodMasteryOverlay foodOverlay;
    [SerializeField]
    Animator anim;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Image food;
    [SerializeField]
    Text ingredients;

    public bool enableStageButtonInput = true;

	void Awake () {
        Time.timeScale = 1;
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
	}

    public void setStage(StageData data) {
        turnOnOverlay();
        GameData.activeStage = data;
        GameData.chosenLevel = data.getMaxLevelUnlocked();
        stageName.text = data.name;
        anim.Play("levelOverlay");
        lvlOverlay.changeChangeButtonsSprite();
    }

    public void setLevel(int level) {
        GameData.chosenLevel = level;
        anim.Play("foodMastery");
        lvlOverlay.changeChangeButtonsSprite();
    }
        
    public void deactivate() {
        if (!enableStageButtonInput) {
            anim.SetTrigger("hide");
            enableStageButtonInput = true;
        }else {
            SceneManager.LoadScene("TitleScreen");
        }
    }

    public void turnOnOverlay() {
        enableStageButtonInput = false;
        panel.SetActive(true);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            PlayerPrefs.DeleteAll();
        }else if (Input.GetKeyDown(KeyCode.A)) {
            SceneManager.LoadScene("MiniGame");
        }
    }

    public void play() {
        if(GameData.chosenLevel == 0) {
            SceneManager.LoadScene("Story");
        } else {
            SceneManager.LoadScene("GameScene");
        }
    }

    public void viewIngredients(int index) {
        anim.Play("viewIngredient");
        food.sprite = GameData.activeStage.food[index].sprite;
        ingredients.text = GameData.activeStage.food[index].getIngredientList();
    }

}
