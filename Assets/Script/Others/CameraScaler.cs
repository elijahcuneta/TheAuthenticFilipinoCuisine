using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour {

    private void Start() {
        float getARQuotient = GetAspectRatioQuotient(Screen.width, Screen.height);
        Camera cam = Camera.main;

        if (getARQuotient >= 2) { //16:9
            cam.orthographicSize = 5;
        } else if (getARQuotient > 1.55f && getARQuotient < 1.7f) { //16:10
            cam.orthographicSize = 5.54f;
        } else if (getARQuotient < 1.55f) { //3:2
            cam.orthographicSize = 5.97f;
        }
    }

    private float GetAspectRatioQuotient (float width, float height) {
        return (width / height);
    }
}
