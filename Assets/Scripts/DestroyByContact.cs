using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    private GameController gameController;

    void Start()
    {
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

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.tag == "Boundary" || other.tag == "PlayerAttack" || other.tag == "Attack" || other.tag == "Boss" || other.tag == "Water")
        {
            return;
        }
        
        Instantiate(explosion, transform.position, transform.rotation);
        if (other.tag == "Player")
        {
            if (!gameController.getTriggerPlayerDeath() && !gameController.getProtectionActive())
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.setAttackHitPlayer(true);
            }            
        }
        Destroy(gameObject);
    }
}
