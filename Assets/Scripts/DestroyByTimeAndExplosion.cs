using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTimeAndExplosion : MonoBehaviour
{
    public AudioClip explosionSound;
    public float lifetime;
    public GameObject explosion;
    public GameObject playerExplosion;

    private GameController gameController;
    private int lifeCount = 0;

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

    private void Update()
    {
        if (lifeCount > 100)
        {
            GameObject gamePlayer = GameObject.FindGameObjectWithTag("Player");
            if (gamePlayer != null)
            {
                Vector3 playerPos = gamePlayer.transform.position;
                float dist = Vector3.Distance(playerPos, transform.position);
                Instantiate(explosion, transform.position, transform.rotation);
                if (dist < 1)
                {
                    gameController.setAttackHitPlayer(true);
                    Instantiate(playerExplosion, gamePlayer.transform.position, gamePlayer.transform.rotation);
                }
            }
            Destroy(gameObject);
        }
        lifeCount++;
    }
}
