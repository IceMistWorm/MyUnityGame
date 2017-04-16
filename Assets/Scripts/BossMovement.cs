using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour {

    public float bossMoveTime;
    public float speed;
    public float tilt;
    private Rigidbody rb;
    private float timeForMove = 3.0f;
    private Vector3 goalPosition = new Vector3(0.0f, 0.0f, 12.5f);
    
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update() {
        timeForMove += Time.deltaTime;
 
        // generate a new goal for the boss to move!
        if (timeForMove >= bossMoveTime)
        {
            goalPosition = new Vector3(Random.Range(-4,4), 0.0f, Random.Range(9.5f,14.5f));
            timeForMove = 0.0f;
        }

        transform.position = Vector3.MoveTowards(transform.position, goalPosition, 0.02f);
        if (transform.position != goalPosition)
        {
            if (transform.position.x - goalPosition.x < 0)
            {
                rb.rotation = Quaternion.Euler(0.0f, 180.0f, (-3f * -tilt));
            }else
            {
                rb.rotation = Quaternion.Euler(0.0f, 180.0f, (3f * -tilt));
            }
        }
        else {
            rb.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
    }
}
