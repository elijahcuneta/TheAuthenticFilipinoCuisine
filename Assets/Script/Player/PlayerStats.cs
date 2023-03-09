using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float[] playerSpeed;
    public float currentPlayerSpeed = 0;

    [HideInInspector]
    public const int playerCanHold = 2;

    public void UpdatePlayerSpeed(int newLevelOfSpeed) {
        if (newLevelOfSpeed >= playerSpeed.Length) {
            newLevelOfSpeed = playerSpeed.Length - 1;
        }
        currentPlayerSpeed = playerSpeed[newLevelOfSpeed];
    }

}
