using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController2 : MonoBehaviour
{

    public GameObject hazard, hazard2, hazard3, hazard4;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public GUIText bosslifeText;
    public GUIText restartText;
    public GUIText gameoverText;
    public GUIText gameclearText;

    public int bosslife;
    private bool gameover;
    private bool gameclear;
    private bool restart;


    void Start()
    {
        //bosslife = 4000;
        bosslifeText.text = "";
        restartText.text = "";
        gameoverText.text = "";
        gameclearText.text = "";
        gameover = false;
        gameclear = false;
        restart = false;
        UpdateBossLife();
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, -8), spawnValues.y, Random.Range(spawnValues.z, spawnValues.z - 8));
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                spawnPosition = new Vector3(Random.Range(spawnValues.x, 8), spawnValues.y, Random.Range(spawnValues.z, spawnValues.z - 8));
                spawnRotation = Quaternion.identity;
                Instantiate(hazard2, spawnPosition, spawnRotation);
                spawnPosition = new Vector3(Random.Range(-9, -4), spawnValues.y, Random.Range(-7.5f, 0.5f));
                spawnRotation = Quaternion.identity;
                Instantiate(hazard3, spawnPosition, spawnRotation);
                spawnPosition = new Vector3(Random.Range(4, 9), spawnValues.y, Random.Range(-7.5f, 0.5f));
                spawnRotation = Quaternion.identity;
                Instantiate(hazard4, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);

                //yield return new WaitForSeconds(spawnWait);
            }

            yield return new WaitForSeconds(waveWait);

            if (gameover || gameclear)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }
    }


    public void BossReduceLife(int lifeReduce)
    {
        bosslife -= lifeReduce;
        if (bosslife < 0)
        {
            bosslife = 0;
        }
        UpdateBossLife();
    }

    public int GetBossLife()
    {
        return bosslife;
    }

    void UpdateBossLife()
    {
        bosslifeText.text = "Boss Life Point: " + bosslife;
    }

    public void GameOver()
    {
        gameoverText.text = "Game Over";
        gameover = true;
    }

    public void GameClear()
    {
        gameoverText.text = "Congratulation! Game Clear!";
        gameclear = true;
    }

    public bool isGameOver()
    {
        return gameover;
    }

    public bool isGameClear()
    {
        return gameclear;
    }
}
