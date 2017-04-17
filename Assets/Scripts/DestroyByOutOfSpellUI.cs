using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByOutOfSpellUI : MonoBehaviour {

    public int spellIndicator;
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
        if (gameController.GetBossSpellTime() < spellIndicator)
        {
            Destroy(gameObject);
        }
    }
}
