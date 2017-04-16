using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBoss : MonoBehaviour {

    public int reduceValue;
    public GameObject bossExplosion;
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

    void OnTriggerEnter(Collider other){
        //Debug.Log(other.name);
        if (other.tag != "PlayerAttack")
        {
            return;
        }
        gameController.BossReduceLife(reduceValue);
        if (gameController.GetBossSpellTime() <= 0)
        {
            Instantiate(bossExplosion, other.transform.position, other.transform.rotation);
            Destroy(gameObject);
            if (gameController.isGameOver() == false)
            {
                gameController.GameClear();
            }
        }else if(gameController.isBossExplode())
        {
            Instantiate(bossExplosion, other.transform.position, other.transform.rotation);
            gameController.setBossExplode(false);
        }

    }
}
