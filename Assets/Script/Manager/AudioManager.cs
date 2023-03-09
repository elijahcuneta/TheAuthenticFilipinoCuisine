using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    [SerializeField]
    public AudioSource angry, click, eating, coin;

	void Awake () {
        instance = this;
	}
}
