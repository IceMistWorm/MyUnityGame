using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByOutofSpellTime : MonoBehaviour
{
    public int spellIndicator;
    public GameObject explosion;
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
    private void Update()
    {
        if (gameController.GetBossSpellTime() != spellIndicator)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
