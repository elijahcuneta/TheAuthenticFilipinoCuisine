using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState {
    OPEN = 0,
    CLOSE = 1,
    WIN = 2,
    LOSE = 3
}

public class GameStateManager : MonoBehaviour {

    private GameState currentGameState = GameState.OPEN;

    [SerializeField]
    private GameObject WinPanel;
    [SerializeField]
    Text scoreDisplay;

    [SerializeField]
    private GameObject LosePanel, TriviaPanel;
    
    private void Start() {
        //if(Instance != this) 
        //    Instance = this;
        // else
        //    Destroy(gameObject);

        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        SetCurrentGameState(GameState.OPEN);
    }

    private void Update() {
        //if close && All table are available && There is no customer in the line
        if (Input.GetKeyDown(KeyCode.M)) {
            LevelWin();
        }

        if (currentGameState == GameState.CLOSE && GameLevelManager.Instance.GetTableSetManager().IsAllTableAvailable() && GameLevelManager.Instance.GetCustomerHandler().IsCustomerLineCleared()) {
            StartCoroutine(ConcludeLevel(2));
        }
    }

    private IEnumerator ConcludeLevel(float delay) {
        yield return new WaitForSeconds(delay);
        ScoreManager scoreManager = GameLevelManager.Instance.GetHUDManager().GetScoreManager();
        if (scoreManager.GetPlayerScore() < scoreManager.GetPlayerGoal()) {
            LevelLose();
        } else {
            LevelWin();
        }
    }

    private void LevelWin() {
        if (GameData.activeStage.incrementLevel()) {
            LevelManager.Instance.GetFoodManager().UpgradeMastery();
            GameData.addCoin((int)GameLevelManager.Instance.GetHUDManager().GetScoreManager().GetPlayerScore());
        }
        WinPanel.SetActive(true);
        scoreDisplay.text = GameLevelManager.Instance.GetHUDManager().GetScoreManager().GetPlayerScore().ToString();
    }
    private void LevelLose() {
        LosePanel.SetActive(true);
    }

    public void SetCurrentGameState(GameState currentGameState) {
        this.currentGameState = currentGameState;
    }
    public GameState GetCurrentGameState() {
        return currentGameState;
    }

    public void StageFinish() {
        //TriviaPanel.SetActive(true);
        //WinPanel.SetActive(true);
        StageQuit();
    }

    public void StageQuit() {
        if (GameData.chosenLevel == 2) {
            if(GameData.activeStage.index == 11) {
                SceneManager.LoadScene("Story");
            }else {
                SceneManager.LoadScene("MiniGame");
            }
        } else {
            SceneManager.LoadScene("LevelSelect");
        }
    }

}
