using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerWaitingSpot : MonoBehaviour {

    [SerializeField]
    TextMesh debug;
    bool newSpotStatus = false;

    private void Start() {
        debug.text = (isSpotTaken ? "Taken" : "not Taken");
    }
    private void Update() {
       if(isSpotTaken != newSpotStatus) {
            debug.text = (isSpotTaken ? "Taken" : "not Taken");
            newSpotStatus = isSpotTaken;
       }
    }

    public int spotId = 0;
    public bool isSpotTaken = false;
    public Transform spotTransfom;

}
