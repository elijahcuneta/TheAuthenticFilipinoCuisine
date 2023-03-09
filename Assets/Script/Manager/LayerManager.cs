using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour {

    public static LayerManager instance;
    
    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void updateLayer(Transform layer) {
        Vector3 temp = layer.transform.position;
        temp.z = temp.y;
        layer.transform.position = temp;
    }
}
