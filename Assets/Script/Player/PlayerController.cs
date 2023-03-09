using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* PlayerController.cs
 * 
 *  Control all Player's Entity 
 *  
 */
public class PlayerController : MonoBehaviour {

    //For debugging purpose only
    public TextMesh textMesh = null;

    void Awake() {
        //if (Instance == null)
        //    Instance = this;
        //else if (Instance != this)
        //    Destroy(gameObject);
    }

    public PlayerInfo playerInfo;
    public PlayerMovement playerMovement;
    public PlayerStats playerStats;
    public Animator animator;
    
}
