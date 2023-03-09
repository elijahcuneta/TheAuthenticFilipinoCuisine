using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStats : MonoBehaviour {

    public float payment = 100f;
    public float eatingSpeed = 50f;
    public float orderingSpeed = 50f;
    public int startingHeartPiece = 3;
    public float patienceValue = 10f; //how many seconds would it take for 1 heart to worn out
    public float fadingPatienceValue = 4f; //what patience value would it need for it to fade
    public float incrementPatienceValue = 2f; //value to increment when customer is entertained

}
