using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{    

     Vector3 characterScale;
	 Vector3 weaponScale;
     float characterScaleX;
	 float weaponScaleX;
	 float weaponScaleY;



    // Start is called before the first frame update
    void Start()
    {
        characterScale = transform.localScale;
        characterScaleX = characterScale.x;
		
		weaponScale = transform.GetChild(0).localScale;
		weaponScaleX = weaponScale.x;
		weaponScaleY = weaponScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        transform.position = transform.position + horizontal * Time.deltaTime;

        Vector3 characterScale = transform.localScale;
        if (Input.GetAxis("Horizontal") < 0 || Input.mousePosition.x < Camera.main.WorldToScreenPoint(transform.position).x) {
			characterScale.x = -characterScaleX;
			weaponScale.x = -weaponScaleX;
			weaponScale.y = -weaponScaleY;
    	}
		if (Input.GetAxis("Horizontal") > 0 || Input.mousePosition.x > Camera.main.WorldToScreenPoint(transform.position).x) {
			characterScale.x = characterScaleX;
			weaponScale.x = weaponScaleX;
			weaponScale.y = weaponScaleY;
		}
		transform.localScale = characterScale;
		transform.GetChild(0).localScale = weaponScale;

    }

   
}