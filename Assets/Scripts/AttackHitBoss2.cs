using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBoss2 : MonoBehaviour
{

    public int reduceValue;
    public GameObject bossExplosion;
    private GameController2 gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController2>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.tag != "PlayerAttack")
        {
            return;
        }
        gameController.BossReduceLife(reduceValue);
        if (gameController.GetBossLife() <= 0)
        {
            Instantiate(bossExplosion, other.transform.position, other.transform.rotation);
            Destroy(gameObject);
            if (gameController.isGameOver() == false)
            {
                gameController.GameClear();
            }
        }

    }
}
