using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour {

    public GameObject waterSplash;

    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Water")
        {
            Instantiate(waterSplash, transform.position, transform.rotation);
        }
    }
}
