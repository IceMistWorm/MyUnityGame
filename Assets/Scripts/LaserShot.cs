using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class LaserShot : MonoBehaviour
{
    Vector2 mouse;
    RaycastHit[] hit;
    LineRenderer line;
    public Material lineMaterial;
    public GameObject playerExplosion;

    private GameObject player;
    private GameController gameController;
    Vector3 laserStart;
    Vector3 laserEnd;
    private bool shotSignal = false;
    private bool laserDeath = false;
    private float accumulateTime = 0.0f;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.numPositions = 2;
        line.GetComponent<LineRenderer>().material = lineMaterial;
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;

        player = GameObject.FindGameObjectWithTag("Player");
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }

    void Update()
    {
        accumulateTime += Time.deltaTime;
        if (accumulateTime >= 3.0f && accumulateTime < 5.0f)
        {

            if (!shotSignal)
            {
                
                line.startWidth = 0.05f;
                line.endWidth = 0.05f;
                laserStart = transform.position;
                laserEnd = player.transform.position;
                Vector3 laserDirection = laserEnd - laserStart;
                laserEnd = laserStart + 3.5f * laserDirection;
                laserEnd.y = 1f;
                Debug.Log("laser end: " + laserEnd + " laser start: " + laserStart);
                line.SetPosition(0, transform.position);
                line.SetPosition(1, laserEnd);
                shotSignal = true;
                line.enabled = true;
            }

        }else if (accumulateTime >= 5.0f && accumulateTime < 7.0f)
        {
            if (!laserDeath)
            {
                
                line.startWidth = 0.5f;
                line.endWidth = 0.5f;
                Vector3 laserDirection = laserEnd - laserStart;
                hit = Physics.RaycastAll(laserStart, laserDirection, 1000f);
                int size = hit.Length;
                for (int i = 0; i < size; i++)
                {
                    //Debug.Log("we hit something " + hit[i].collider.tag);
                    Debug.Log("laser start: " + laserStart.y + " laser direction: " + laserDirection.y);
                    if (hit[i].collider.tag == "Player")
                    {
                        laserDeath = true;
                        Instantiate(playerExplosion, player.transform.position, player.transform.rotation);
                        gameController.setAttackHitPlayer(true);
                    }
                }
            }
                
        }
        else if(accumulateTime >= 7.0f)
        {
            laserDeath = false;
            line.enabled = false;
            shotSignal = false;
            accumulateTime = 0.0f;
        }

    }
}
