using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour {

    private Sprite location;
    private Sprite floor;

    public void SetSpriteLocation(Sprite location) {
        this.location = location;
    }
    public Sprite GetSpriteLocation() {
        return location;
    }

    public void SetSpriteFloor(Sprite floor) {
        this.floor = floor;
    }
    public Sprite GetSpriteFloor() {
        return floor;
    }
}
