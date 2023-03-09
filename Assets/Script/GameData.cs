using System;
using UnityEngine;

[Serializable]
public class FoodData {
    public const int MAX = 5;
    public string nameID;
    public Sprite sprite;
    public string[] ingredients;
    [SerializeField]
    int startingPrice;
    int mastery;
    
    public int getMastery() {
        return mastery;
    }

    public int getPrice() {
        return Mathf.RoundToInt(startingPrice * (mastery+1) * 0.5f);    
    }

    public void Load() {
        mastery = PlayerPrefs.GetInt(nameID, 0);
    }
    
    public void upgradeMastery() {
        PlayerPrefs.SetInt(nameID, ++mastery);
        PlayerPrefs.Save();
    }

    public string getIngredientList() {
        string temp = "";
        for(int i = 0; i < ingredients.Length; i++) {
            temp += ingredients[i] + "\n";
        }
        return temp;

    }
}

[Serializable]
public class StageData {
    public int index;//ID 
    public string name, trivia, lastTrivia;
    int level; // 0-4
    public Sprite background, floor;
    public FoodData[] food;

    public float[] goal, customerRespawnTimer;
    public int[] timer;
    public string[] story;

    public float miniGameTime = 250;
    public string[] choices;

    public int getMaxLevelUnlocked() {
        return level;
    }

    public void Load() {
        level = PlayerPrefs.GetInt(index.ToString(), 0);
        foreach (FoodData data in food) {
            data.Load();
        }
    }

    public FoodData GetFood(int index) {
        return food[index];
    }

    public bool incrementLevel() {
        if (GameData.chosenLevel == GameData.activeStage.getMaxLevelUnlocked()) {
            PlayerPrefs.SetInt(index.ToString(), ++level);
            if (level > 2) {
                GameData.incrementStage();
            }
            return true;
        }
        return false;
    }

    public Sprite GetBackground() {
        return background;
    }

    public Sprite GetFloor() {
        return floor;
    }
}

public static class GameData {
    
    public static StageData activeStage;

    static int coin;
    static int stageReached;
    public static int chosenLevel;
    static int tableCountLevel, cookingTimeLevel;


    static GameData() {
        coin = PlayerPrefs.GetInt("coin", 0);
        stageReached = PlayerPrefs.GetInt("stage", 0);
        tableCountLevel = PlayerPrefs.GetInt("tableUpg", 0);
        cookingTimeLevel = PlayerPrefs.GetInt("cookUpg", 0);
    }

    public static int getMaxStageReached() {
        return stageReached;
    }

    public static int getCoin() {
        return coin;
    }

    public static void addCoin(int collected, bool save = true) {
        coin += collected;
        if (save) {
            PlayerPrefs.SetInt("coin", coin);
            PlayerPrefs.Save();
        }
    }

    public static void incrementStage() {
        PlayerPrefs.SetInt("stage", ++stageReached); 
    }

    public static void incrementTable() {
        PlayerPrefs.SetInt("tableUpg", ++tableCountLevel);
    }
    public static void incrementCookingTime() {
        PlayerPrefs.SetInt("cookUpg", ++cookingTimeLevel);
    }

    public static int getTableCount() {
        return tableCountLevel +  3;
    }
    
    public static float getCookingTime() {
        return 5 - cookingTimeLevel * 0.5f;
    }

    public static float getTablePercent() {
        return (float)(tableCountLevel+1) / 3f;
    }

    public static float getCookingTimePercent() {
        return (float)(cookingTimeLevel+1) / 5f;
    }

}
