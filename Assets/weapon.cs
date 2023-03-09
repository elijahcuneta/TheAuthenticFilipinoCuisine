using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour {

    public GameObject projectile;
    public Transform shotPoint;
    
    private float timeBtwshots;
    public float startTimeBtwshots;

   private void Update()
   {
       Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
       float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
       transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    
    //    if (timeBtwshots <= 0) {
    // //       if (Input.GetMouseButtonDown(0)) {
    // //          Instantiate(projectile, shotPoint.position, transform.rotation);
    // //    }     timeBtwshots = startTimeBtwshots;
    

    //    }
   
    // else {
    //     timeBtwshots -= Time.deltaTime;
    // }   
   

   } 
    
}