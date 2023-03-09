using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    [SerializeField]
    private TextMesh textMesh = null;

    [HideInInspector]
    public int noteId = 0;

    [HideInInspector]
    public CustomerBatch customerBatch = null;

    [HideInInspector]
    public int handPosition = 0;

    public void SetOrder(int noteId, CustomerBatch customerBatch) {
        textMesh.text = noteId.ToString();
        this.noteId = noteId;
        this.customerBatch = customerBatch;
    }

    public void SetLocalPosition_WhenInPlayer(int index, Transform[] playerTransform = null) {
        if (playerTransform != null) {
            transform.parent = playerTransform[index];
        }
        if (index >= PlayerStats.playerCanHold) {
            index = PlayerStats.playerCanHold - 1; //not static
        }
        handPosition = index;
        transform.localPosition = Vector3.zero;
    }

    private void Update() {
        transform.eulerAngles = Vector3.zero;
    }

    public void DestroyNote() {
        Destroy(gameObject);
    }
}
