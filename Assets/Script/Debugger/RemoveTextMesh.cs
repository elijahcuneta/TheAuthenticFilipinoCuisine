using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveTextMesh : MonoBehaviour {

    [SerializeField]
    bool debug;

    private void Start() {
        if (!debug) {
            Destroy(gameObject);
        }
    }

    private void LateUpdate() {
        TextMesh[] tm = FindObjectsOfType<TextMesh>();
        for(int i = 0; i < tm.Length; i++) {
            tm[i].color = new Color(0, 0, 0, 0);
        }
    }
}
