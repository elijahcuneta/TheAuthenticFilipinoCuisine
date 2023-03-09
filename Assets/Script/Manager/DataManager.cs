using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    private float goal;
    private int time;
    private int tableCount;
    private float cookingTime;
    private float customerRespawnTimer;

    public void SetLevelGoal(float goal) {
        this.goal = goal;
    }
    public float GetLevelGoal() {
        return goal;
    }

    public void SetLevelTime(int time) {
        this.time = time;
    }
    public int GetLevelTime() {
        return time;
    }

    public void SetTableCount(int tableCount) {
        this.tableCount = tableCount;
    }
    public int GetTableCount() {
        return tableCount;
    }

    public void SetCookingTime(float cookingTime) {
        this.cookingTime = cookingTime;
    }
    public float GetCookingTime() {
        return cookingTime;
    }

    public void SetCustomerRespawnTimer(float customerRespawnTimer) {
        this.customerRespawnTimer = customerRespawnTimer;
    }
    public float GetCustomerRespawnTimer() {
        return customerRespawnTimer;
    }
}
