using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public float speedX;
    public float speedZ;
    public float speedY;

    private Rigidbody rb;
    private Vector3 updatePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();


        //rb.velocity = new Vector3(Random.Range(speedX, speedX+1),0, Random.Range(speedZ, speedZ + 2));
        if (speedY != 0)
        {
            rb.velocity = new Vector3(0, Random.Range(speedY, speedY + 1), 0);
        }
        else {
            //rb.velocity = new Vector3(Random.Range(speedX, speedX + 1), 0, Random.Range(speedZ, speedZ + 2));
            rb.velocity = new Vector3(speedX, 0, speedZ);
        }
        


    }
}
