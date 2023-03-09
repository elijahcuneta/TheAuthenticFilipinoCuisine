using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    [SerializeField]
    private Text playerScore;

    [SerializeField]
    Slider slider;

    private float score;
    private float goal = 25000;

    private void Awake() {
    }

    public void AddPlayerScore(float addScore) {
        score += addScore;
        playerScore.text = "Score: " + score + " / " + goal;
        slider.value = score / goal;
    }

    public void SetPlayerScore(float newScore) {
        playerScore.text = "Score: " + newScore + " / " + goal;
        slider.value = newScore / goal;
    }
    public float GetPlayerScore() {
        return score;
    }

    public void SetPlayerGoal(float goal) {
        this.goal = goal;
        SetPlayerScore(score);
    }
    public float GetPlayerGoal() {
        return goal;
    }

    public void HidePlayerScore() {
        slider.gameObject.SetActive(false);
    }
    public void ShowPlayerScore() {
        slider.gameObject.SetActive(true);
    }
}
