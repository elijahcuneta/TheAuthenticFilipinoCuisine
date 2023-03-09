using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour {

    int index;

    [SerializeField]
    Text display;

    [SerializeField]
    string[] story;

    [SerializeField]
    Animator character;

    private void Awake() {
        if (GameData.getMaxStageReached() < 12 && GameData.activeStage != null) {
            story = GameData.activeStage.story;
        }
        show();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            index++;
            character.Play("Talk");
            show();
        }
    }

    void show() {
        if (index == story.Length) {
            if (GameData.getMaxStageReached() < 12) {
                SceneManager.LoadScene("GameScene");
            }else {
                SceneManager.LoadScene("TitleScreen");
            }
        } else {
            display.text = story[index];
        }
    }
}
