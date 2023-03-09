using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelAssetsManager : MonoBehaviour {

    [SerializeField]
    private Image floor = null;
    [SerializeField]
    private Image location = null;

    public void InitializeAssets() {
        location.sprite = LevelManager.Instance.GetAssetManager().GetSpriteLocation();
        floor.sprite = LevelManager.Instance.GetAssetManager().GetSpriteFloor();
    }
}
