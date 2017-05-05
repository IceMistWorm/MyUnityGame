using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}


public class PlayerController : MonoBehaviour {

    public float speed;
    public float tilt;
    public Boundary boundary;
    public GameObject protection;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private GameController gameController;
    private float nextFire;
    private Rigidbody rb;
    private bool protectionActive = false;
    private float cannotDeathTime = 2.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void Update()
    {
        if (protectionActive)
        {
            cannotDeathTime -= Time.deltaTime;
            if (cannotDeathTime <= 0.0f)
            {
                protectionActive = false;
                protection.SetActive(false);
                cannotDeathTime = 2.5f;
            }
        }

        if (Input.GetKey("z") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }

        if (Input.GetKey("x") && !protectionActive && gameController.getProtectionActive())
        {
            protectionActive = true;
            protection.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = (movement * speed);

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, (rb.velocity.x * -tilt));
    }

}
