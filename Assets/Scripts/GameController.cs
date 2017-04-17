﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject hazard, hazard2, hazard3, hazard4,hazard5;
    public GameObject greatMosaicWall, bigAnnoyingBall;
    public GameObject spellCardScene;
    public Light dirRedLight;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    

    public AudioClip bossSpellCard;
    public AudioClip bossBasicAttack1;
    public AudioClip bossAttack1;
    public AudioClip bossAttack2;
    public AudioClip bossAttack3;
    public AudioClip bossDefeated;
    public AudioClip playerDeath;
    public AudioClip horn;

    public GUIText bosslifeText;
    public GUIText restartText;
    public GUIText gameoverText;
    public GUIText gameclearText;

    public Slider BossHealthBar;
    public Text SpellCardName;
    public RawImage robotImage;

    public int bosslife;
    public int bossSpell;

    private AudioSource source;
    private bool gameover;
    private bool gameclear;
    private bool restart;
    private bool bossExplode = false;

    private int waves;
    private int newHazardCount;
    private int random_skip = 3;
    private float basicAttackTime1 = 2.5f;
    private float spell1_time = 2.5f;
    private float spellRockWater = 2.5f;
    private float spell3_time = 4.5f;
    // use for spell card 5 to generate a mosaic ball every 6 sec
    private float accumulateTime = 0.0f;

    private float bossSummonBallTime = 6.0f;
    private int bossBasicLife = 8000;
    private int bossPreviousSpell;
    private bool randomSkipIncrease = true;
    private bool attackHitPlayer = false;
    private bool wallBuilt = false;
    private GameObject boss;

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
        bossPreviousSpell = bossSpell;
        boss = GameObject.FindGameObjectWithTag("Boss");
        dirRedLight.enabled = false;
        spellCardScene.SetActive(false);
        robotImage.enabled = false;
 
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
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
        if (attackHitPlayer) {
            attackHitPlayer = false;
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            source.PlayOneShot(playerDeath);
            if (isGameClear() == false)
            {
                GameOver();
            }
        }
        if(bossSpell == 6)
        {
            //dirRedLight.enabled = false;
            //spellCardScene.SetActive(false);
        }
        else if(bossSpell == 5)
        {
            //dirRedLight.enabled = true;
            //spellCardScene.SetActive(true);
            accumulateTime += Time.deltaTime;
            float attackRatio = (float)bosslife / (float)bossBasicLife;
            bossSummonBallTime = 6.0f * attackRatio;
            if (bossSummonBallTime <= 3.0f)
            {
                bossSummonBallTime = 3.0f;
            }
            if (accumulateTime >= bossSummonBallTime)
            {
                source.PlayOneShot(horn,0.85f);
                Vector3 spawnPosition = new Vector3(boss.transform.position.x, boss.transform.position.y, boss.transform.position.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(bigAnnoyingBall, spawnPosition, spawnRotation);
                accumulateTime = 0;
            }
        }else if(bossSpell == 4)
        {
            //dirRedLight.enabled = false;
            //spellCardScene.SetActive(false);

        }
        else if (bossSpell == 3)
        {
            //dirRedLight.enabled = true;
            //spellCardScene.SetActive(true);
        }
        else if (bossSpell == 2)
        {
            //dirRedLight.enabled = false;
            //spellCardScene.SetActive(false);
        }
        else if (bossSpell == 1)
        {
            //dirRedLight.enabled = true;
            //spellCardScene.SetActive(true);
        }
        

        BossHealthBar.value = calculateBossHealthRatio();
        
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        Vector3 spawnPosition;
        Quaternion spawnRotation;
        waves = 0;
        while (true)
        {

            if (waves == 0)
            {
                newHazardCount = hazardCount;
            }
            else if (waves == 1)
            {
                newHazardCount = hazardCount;
            }
            else if (waves == 2)
            {
                newHazardCount = hazardCount;
            }
            else if (waves == 3)
            {
                newHazardCount = hazardCount;
            }
            else if (waves == 4) {
                newHazardCount = hazardCount;
            }

            for (int i = 0; i < newHazardCount; i++)
            {
                
                float position_x, position_z;

                // no generation attack if game is over or clear
                if (isGameOver())
                {
                    break;
                }

                /* if spell card change, wait a few second before continue*/

                if (bossPreviousSpell != bossSpell) {
                    bossPreviousSpell = bossSpell;
                    SpellCardName.text = "";
                    if (bossSpell % 2 == 1)
                    {
                        yield return new WaitForSeconds(1.5f);
                        updateSpellCardText();
                        robotImage.enabled = true;
                        source.PlayOneShot(bossSpellCard);
                        dirRedLight.enabled = true;
                        spellCardScene.SetActive(true);
                    }
                    else {                        
                        dirRedLight.enabled = false;
                        spellCardScene.SetActive(false);
                        yield return new WaitForSeconds(2.5f);
                        updateSpellCardText();
                    }
                    yield return new WaitForSeconds(2.5f);
                    
                    robotImage.enabled = false;
                    break;
                }


                /*
                 *  This is the normal attack from the boss (before spell card 1)
                 *  which sends a basic rock pattern and player just need to move left and right to dodge
                 *  
                 */
                if (bossSpell == 6)
                {

                }

                /**
                 *   Boss Spell Card: Big annoy ball
                 *
                 */

                else if (bossSpell == 5)
                {
                    if (i % 11 == 0)
                    {
                        source.PlayOneShot(bossBasicAttack1, 1f);
                        if (basicAttackTime1 > 0.75f)
                        {
                            float attackRatio = (float)bosslife / (float)bossBasicLife;
                            basicAttackTime1 = 2.5f * attackRatio;
                        }
                        if (basicAttackTime1 < 0.75f)
                        {
                            basicAttackTime1 = 0.75f;
                        }
                        yield return new WaitForSeconds(basicAttackTime1);
                    }
                    position_x = Random.Range(-7, 7);
                    position_z = Random.Range(25, 40);
                    spawnPosition = new Vector3(position_x, 0.5f, position_z);
                    spawnRotation = Quaternion.identity;
                    Instantiate(hazard5, spawnPosition, spawnRotation);
                }
                /*
                 *  This is the normal attack from the boss (before spell card 1)
                 *  which sends a basic rock pattern and player just need to move left and right to dodge
                 *  
                 */

                else if (bossSpell == 4) {

                    // entering the spell: the great mosaic wall
                    if (!wallBuilt)
                    {
                        source.PlayOneShot(horn, 0.85f);
                        yield return new WaitForSeconds(1.0f);
                        wallBuilt = true;
                        spawnRotation = Quaternion.identity;
                        for (int j = 0; j < 5; j++)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                spawnPosition = new Vector3(-10 + j * 5, -5 + k * 5, 7);
                                Instantiate(greatMosaicWall, spawnPosition, spawnRotation);
                            }
                        }
                    }

                    if (i % 11 == 0)
                    {
                        source.PlayOneShot(bossBasicAttack1, 1f);
                        if (basicAttackTime1 > 0.75f)
                        {
                            float attackRatio = (float)bosslife / (float)bossBasicLife;
                            basicAttackTime1 = 2.5f * attackRatio;
                        }
                        if (basicAttackTime1 < 0.75f)
                        {
                            basicAttackTime1 = 0.75f;
                        }
                        yield return new WaitForSeconds(basicAttackTime1);
                    }
                    position_x = Random.Range(-7, 7);
                    position_z = Random.Range(25, 40);
                    spawnPosition = new Vector3(position_x, 0.5f, position_z);
                    spawnRotation = Quaternion.identity;
                    Instantiate(hazard4, spawnPosition, spawnRotation);
                }                
                /*
                 *  Spell card No1: (to be named)
                 *  rocks emerges under the water, player needs to dodge them base on the shadow shown on the water.
                 * 
                 */
                else if (bossSpell == 3)
                {
                    // generate 20 rocks to attack every time
                    if (i % 20 == 0)
                    {
                        source.PlayOneShot(bossAttack1, 1f);
                        if (spellRockWater > 0.75f)
                        {
                            float attackRatio = (float)bosslife / (float)bossBasicLife;
                            spellRockWater = 2.5f * attackRatio;
                        }
                        if (spellRockWater < 0.75f)
                        {
                            spellRockWater = 0.75f;
                        }
                        yield return new WaitForSeconds(spellRockWater);
                    }
                    spawnPosition = new Vector3(Random.Range(-7, 7), spawnValues.y, Random.Range(-7.5f, 7.5f));
                    //position_x = -7 + i % 14;
                    //position_z = -7.5f + i % 15;
                    //spawnPosition = new Vector3(position_x, spawnValues.y, position_z);
                    spawnRotation = Quaternion.identity;
                    Instantiate(hazard, spawnPosition, spawnRotation);
                    
                }
                /*
                 *  This is the normal attack from the boss
                 *  which forms a basic rock pattern and player just need to move left and right to dodge
                 *  simple but a fast spell 
                 * 
                 */
                else if (bossSpell == 2)
                {
                    
                    //random_skip = 4 + (i/14)%6;
                    
                    if (i % 14 == 0) {

                        if (randomSkipIncrease)
                        {
                            random_skip++;
                            if (random_skip >= 12)
                            {
                                randomSkipIncrease = false;
                            }
                        }
                        else
                        {
                            random_skip--;
                            if (random_skip <= 3)
                            {
                                randomSkipIncrease = true;
                            }
                        }

                        if (spell1_time > 0.15f)
                        {
                            //spell1_time -= (i/14)*0.1f;
                            float attackRatio = (float)bosslife / (float)bossBasicLife;
                            spell1_time = 2.5f * attackRatio;
                        }
                        if (spell1_time < 0.15f)
                        {
                            spell1_time = 0.15f;
                        }
                        yield return new WaitForSeconds(spell1_time);
                        source.PlayOneShot(bossAttack2);

                    }

                    if (i % 14 <= random_skip && i % 14 >= random_skip-1)
                    {
                        //Debug.Log("should have this in time increase " + i/14);
                        continue;
                    }

                    position_x = -7 + i % 14;
                    position_z = 25;
                    spawnPosition = new Vector3(position_x, 0.5f, position_z);
                    spawnRotation = Quaternion.identity;
                    Instantiate(hazard2, spawnPosition, spawnRotation);
                }
                else if (bossSpell == 1)
                {
                    if (i % 20 == 0)
                    {
                        if (spell3_time > 1f)
                        {
                            float attackRatio = (float)bosslife / (float)bossBasicLife;
                            spell3_time = 4.5f * attackRatio;
                        }
                        if (spell3_time < 1f)
                        {
                            spell3_time = 1f;
                        }
                        yield return new WaitForSeconds(spell3_time);
                        source.PlayOneShot(bossAttack3, 0.7f);
                    }
                    spawnPosition = new Vector3(Random.Range(-7, 7), 0.5f, Random.Range(-7.5f, 7.5f));
                    spawnRotation = Quaternion.identity;
                    Instantiate(hazard3, spawnPosition, spawnRotation);
                    
                }
                yield return new WaitForSeconds(spawnWait);
            }


            if (waves < 4)
            {
                waves++;
            }
            else {
                waves = 0;
            }

            //yield return new WaitForSeconds(waveWait);

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
            bosslife = bossBasicLife;
            setBossExplode(true);
            bossSpell--;
            source.PlayOneShot(bossDefeated, 0.2f);
            if (bossSpell==0){
                bosslife = 0;
            }
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

    public int GetBossSpellTime()
    {
        return bossSpell;
    }



    public void GameOver()
    {
        gameoverText.text = "Game Over";
        gameover = true;
    }

    public void GameClear()
    {
        SpellCardName.text = "";
        Destroy(BossHealthBar);
        gameoverText.text = "Congratulation! Game Clear!";
        spellCardScene.SetActive(false);
        dirRedLight.enabled = false;
        source.PlayOneShot(bossDefeated, 0.5f);
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

    public bool isBossExplode()
    {
        return bossExplode;
    }

    public void setBossExplode(bool v)
    {
        bossExplode = v;
    }

    public void setAttackHitPlayer(bool v) {
        attackHitPlayer = v;
    }

    private float calculateBossHealthRatio() {
        return (float)bosslife / (float)bossBasicLife;
    }
    private void updateSpellCardText() {
        if (bossSpell == 6)
        {
            SpellCardName.text = "Test";
        }else if(bossSpell == 5)
        {
            SpellCardName.text = "[Earth Spell] Natural Mosaic";
        }
        else if (bossSpell == 4)
        {
            SpellCardName.text = "The Great Refraction Wall";
        }
        else if (bossSpell == 3)
        {
            SpellCardName.text = "[Earth Spell] Rockarine";
        }
        else if (bossSpell == 2)
        {
            SpellCardName.text = "Earthutation";
        }
        else if (bossSpell == 1)
        {
            SpellCardName.text = "[Thunder Spell] Particle Overflow";
        }
    }
}
