using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance = null;

    [SerializeField]
    private FoodManager foodManager = null;
    [SerializeField]
    private AssetManager assetManager = null;
    [SerializeField]
    private DataManager dataManager = null;

    private void Awake() {
        if (Instance != this)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public FoodManager GetFoodManager() {
        return foodManager;
    }

    public AssetManager GetAssetManager() {
        return assetManager;
    }

    public DataManager GetDataManager() {
        return dataManager;
    }
}
