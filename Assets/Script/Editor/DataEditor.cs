using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataEditor : Editor {

    

    [MenuItem("Tools/Clear Player Prefs")]
    private static void ClearPlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Add 100 Coins")]
    private static void Add100Coins() {
        int coins = PlayerPrefs.GetInt("coin", 0);
        coins += 100;
        PlayerPrefs.SetInt("coin", coins);
        PlayerPrefs.Save();
    }

    [MenuItem("Tools/Subtract 100 Coins")]
    private static void Sub100Coins() {
        int coins = PlayerPrefs.GetInt("coin", 0);
        coins -= 100;
        coins = coins < 0 ? 0 : coins;
        PlayerPrefs.SetInt("coin", coins);
        PlayerPrefs.Save();
    }

    [MenuItem("Tools/Add Table Count")]
    private static void AddTableCount() {
        int tableCountLevel = PlayerPrefs.GetInt("tableUpg", 0);
        PlayerPrefs.SetInt("tableUpg", ++tableCountLevel > 4 ? 4 : tableCountLevel);
        PlayerPrefs.Save();
    }

    [MenuItem("Tools/Subtract Table Count")]
    private static void SubTableCount() {
        int tableCountLevel = PlayerPrefs.GetInt("tableUpg", 0);
        PlayerPrefs.SetInt("tableUpg", --tableCountLevel < 0 ? 0 : tableCountLevel);
        PlayerPrefs.Save();
    }

    [MenuItem("Tools/Add Cooking Time")]
    private static void AddCookingTime() {
        int cookingTimeLevel = PlayerPrefs.GetInt("cookUpg", 0);
        PlayerPrefs.SetInt("cookUpg", ++cookingTimeLevel > 4 ? 4 : cookingTimeLevel);
        PlayerPrefs.Save();
    }

    [MenuItem("Tools/Subtract Cooking Time")]
    private static void SubCookingTime() {
        int cookingTimeLevel = PlayerPrefs.GetInt("cookUpg", 0);
        PlayerPrefs.SetInt("cookUpg", --cookingTimeLevel < 0 ? 0 : cookingTimeLevel);
        PlayerPrefs.Save();
    }

    [MenuItem("Tools/Add Stage")]
    private static void AddStage() {
        int stage = PlayerPrefs.GetInt("stage", 0);
        PlayerPrefs.SetInt("stage", ++stage > 9 ? 9 : stage);
        PlayerPrefs.Save();
    }

    [MenuItem("Tools/Subtract Stage")]
    private static void SubStage() {
        int stage = PlayerPrefs.GetInt("stage", 0);
        PlayerPrefs.SetInt("stage", --stage < 0 ? 0 : stage);
        PlayerPrefs.Save();
    }

    [MenuItem("Tools/Fast motion")]
    private static void Fast_Motion() {
        Time.timeScale = 2f;
    }

    [MenuItem("Tools/Normal Motion")]
    private static void Norm_Motion() {
        Time.timeScale = 1f;
    }

    [MenuItem("Tools/Slow Motion")]
    private static void Slow_Motion() {
        Time.timeScale = 0.5f;
    }

    [MenuItem("Tools/Reset Settings")]
    private static void Reset() {
        Time.timeScale = 1f;
        ClearPlayerPrefs();
    }
}
