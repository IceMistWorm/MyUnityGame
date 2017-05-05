﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject hazard, hazard2, hazard3, hazard4,hazard5,hazard6,hazard7,hazard8,mistyCosmicAttack, mistyCosmicAttack2, mistyCosmicAttack3;
    public GameObject greatMosaicWall, bigAnnoyingBall, attackWithBall;
    public GameObject laserBall,laserBall2;
    public GameObject spellCardScene;
    public GameObject MistyEffect,MistyEffect2, MistyEffect3;
    public Light dirRedLight;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public int playerLife;

    public AudioClip bossSpellCard;
    public AudioClip bossBasicAttack1;
    public AudioClip bossAttack1;
    public AudioClip bossAttack2;
    public AudioClip bossAttack3;
    public AudioClip bossDefeated;
    public AudioClip playerDeath;
    public AudioClip horn;
    public AudioClip wave;

    public GUIText bosslifeText;
    public GUIText restartText;
    public GUIText gameoverText;
    public GUIText gameclearText;
    public Text playerScore;
    public Text bonusScore;
    public Text playerLifeText;

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
    private float basicAttackTime2 = 3.5f;
    private float basicAttackTime3 = 3.5f;
    private float spell1_time = 2.5f;
    private float spellRockWater = 2.5f;
    private float spell3_time = 4.5f;
    // use for spell card 5 to generate a mosaic ball every 6 sec
    private float accumulateTime = 0.0f;

    private float bossSummonBallTime = 6.0f;

    private int bossBasicLife = 8000;
    private int bossFinalLife = 16000;
    private int bossPreviousSpell;
    private bool randomSkipIncrease = true;
    private bool attackHitPlayer = false;
    private bool wallBuilt = false;
    private bool spell2LaserBallBuilt = false;
    private bool spell1LaserBallBuilt = false;
    private GameObject boss;
    private GameObject player;
    private bool triggerPlayerDeath = false;
    private float playerDeathTime = 3.0f;
    private bool protectionActive = false;
    private float cannotDeathTime = 2.5f;
    private int score;
    private bool useProtection = false;
    private bool playerDeathInThisStage = false;

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
        score = 10000;
        UpdateBossLife();
        StartCoroutine(SpawnWaves());
        bossPreviousSpell = bossSpell;
        boss = GameObject.FindGameObjectWithTag("Boss");
        player = GameObject.FindGameObjectWithTag("Player");
        dirRedLight.enabled = false;
        spellCardScene.SetActive(false);
        robotImage.enabled = false;
        updateSpellCardText();
        updateScoreText();
        updateBonusScoreText();
        updatePlayerLifeText();
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        // game restart
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
        // if attack hit the player
        if (attackHitPlayer) {
            attackHitPlayer = false;
            //Destroy(GameObject.FindGameObjectWithTag("Player"));
            if (!triggerPlayerDeath && !protectionActive)
            {
                triggerPlayerDeath = true;
                playerDeathInThisStage = true;
                updateBonusScoreText();
                source.PlayOneShot(playerDeath);
                if (isGameClear() == false)
                {
                    if (playerLife == 0)
                    {
                        GameOver();
                    }else
                    {
                        playerLife--;
                        updatePlayerLifeText();
                    }
                }
            }
        }
        //

        if (protectionActive)
        {
            cannotDeathTime -= Time.deltaTime;
            if (cannotDeathTime <= 0.0f)
            {
                protectionActive = false;
                cannotDeathTime = 2.5f;
            }
        }
        if (Input.GetKey("x") && !protectionActive && !gameover && !gameclear)
        {
            if (score >= 5000)
            {
                score -= 5000;
                protectionActive = true;
                useProtection = true;
                updateScoreText();
                updateBonusScoreText();
            }
        }

        if (bossSpell == 7)
        {
            //dirRedLight.enabled = true;
            //spellCardScene.SetActive(true);
            accumulateTime += Time.deltaTime;
            float attackRatio = (float)bosslife / (float)bossBasicLife;
            bossSummonBallTime = 6.0f * attackRatio;
            if (bossSummonBallTime <= 1.0f)
            {
                bossSummonBallTime = 1.0f;
            }
            if (accumulateTime >= bossSummonBallTime)
            {
                source.PlayOneShot(horn, 0.85f);
                Vector3 spawnPosition = new Vector3(boss.transform.position.x, boss.transform.position.y +20 , boss.transform.position.z-10);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(bigAnnoyingBall, spawnPosition, spawnRotation);
                spawnPosition = new Vector3(boss.transform.position.x * -1, boss.transform.position.y + 20, boss.transform.position.z - 8);
                Instantiate(attackWithBall, spawnPosition, spawnRotation);
                accumulateTime = 0;
            }
            /*
            if (!spell7LaserBallBuilt && accumulateTime >= 4.0f)
            {
                spell7LaserBallBuilt = true;
                source.PlayOneShot(horn, 0.85f);
                Quaternion spawnRotation = Quaternion.identity;
                Vector3 spawnPosition1 = new Vector3(3.3f, 1.0f, 15.0f);
                Vector3 spawnPosition2 = new Vector3(10.0f, 1.0f, 15.0f);
                Vector3 spawnPosition3 = new Vector3(-3.3f, 1.0f, 15.0f);
                Vector3 spawnPosition4 = new Vector3(10.0f, 1.0f, 15.0f);
                Instantiate(laserBall, spawnPosition1, spawnRotation);
                Instantiate(laserBall, spawnPosition2, spawnRotation);
                Instantiate(laserBall, spawnPosition3, spawnRotation);
                Instantiate(laserBall, spawnPosition4, spawnRotation);
            }
            */
        }
        else if(bossSpell == 6)
        {
            
        }
        else if (bossSpell == 5)
        {
            //dirRedLight.enabled = false;
            //spellCardScene.SetActive(false);
            bossSummonBallTime = 6.0f;
        }
        else if(bossSpell == 4)
        {
            accumulateTime += Time.deltaTime;
            float attackRatio = (float)bosslife / (float)bossBasicLife;
            bossSummonBallTime = 4.0f * attackRatio;
            if (bossSummonBallTime <= 1.0f)
            {
                bossSummonBallTime = 1.0f;
            }
            if (accumulateTime >= bossSummonBallTime)
            {
                source.PlayOneShot(wave, 0.85f);
                Vector3 spawnPosition = new Vector3(boss.transform.position.x, 0.5f, boss.transform.position.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(mistyCosmicAttack, spawnPosition, spawnRotation);
                accumulateTime = 0;
            }

        }
        else if (bossSpell == 3)
        {
            //dirRedLight.enabled = true;
            //spellCardScene.SetActive(true);
        }
        // misty cosmic attack update
        else if (bossSpell == 2)
        {
            accumulateTime += Time.deltaTime;
            float attackRatio = (float)bosslife / (float)bossBasicLife;
            bossSummonBallTime = 4.0f * attackRatio;
            if (bossSummonBallTime <= 1.0f)
            {
                bossSummonBallTime = 1.0f;
            }
            if (accumulateTime >= bossSummonBallTime)
            {
                source.PlayOneShot(wave, 0.85f);
                Vector3 spawnPosition = new Vector3(boss.transform.position.x, 0.5f, boss.transform.position.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(mistyCosmicAttack2, spawnPosition, spawnRotation);
                accumulateTime = 0;
            }
            if (!spell2LaserBallBuilt && accumulateTime > 2.0f)
            {
                spell2LaserBallBuilt = true;
                source.PlayOneShot(horn, 0.85f);
                Quaternion spawnRotation = Quaternion.identity;
                Vector3 spawnPosition1 = new Vector3(0.0f, 1.0f, 15.0f);
                Instantiate(laserBall2, spawnPosition1, spawnRotation);
            }
        }
        else if (bossSpell == 1)
        {
            if ((float)bosslife / (float)bossFinalLife <= 0.6)
            { 
                accumulateTime += Time.deltaTime;
                float attackRatio = (float)bosslife / (float)bossFinalLife;
                bossSummonBallTime = 4.0f * attackRatio;
                if (bossSummonBallTime <= 1.0f)
                {
                    bossSummonBallTime = 1.0f;
                }
                if (accumulateTime >= bossSummonBallTime)
                {
                    source.PlayOneShot(wave, 0.85f);
                    Vector3 spawnPosition = new Vector3(boss.transform.position.x, 0.5f, boss.transform.position.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(mistyCosmicAttack3, spawnPosition, spawnRotation);
                    accumulateTime = 0;
                }
            }

            if (!spell1LaserBallBuilt && (float)bosslife / (float)bossFinalLife <= 0.4)
            {
                spell1LaserBallBuilt = true;
                source.PlayOneShot(horn, 1f);
                Quaternion spawnRotation = Quaternion.identity;
                Vector3 spawnPosition1 = new Vector3(0.0f, 1.0f, 15.0f);
                Vector3 spawnPosition2 = new Vector3(-10.0f, 1.0f, 15.0f);
                Vector3 spawnPosition3 = new Vector3(10.0f, 1.0f, 15.0f);
                Instantiate(laserBall, spawnPosition1, spawnRotation);
                Instantiate(laserBall, spawnPosition2, spawnRotation);
                Instantiate(laserBall, spawnPosition3, spawnRotation);

            }
        }
        

        BossHealthBar.value = calculateBossHealthRatio();
        if (triggerPlayerDeath && playerDeathTime>0.0f)
        {
            playerDeathTime -= Time.deltaTime;
        }else if (triggerPlayerDeath)
        {
            triggerPlayerDeath = false;
        }
        else
        {
            playerDeathTime = 3.0f;
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        Vector3 spawnPosition;
        Quaternion spawnRotation;
        waves = 0;
        while (true)
        {
            newHazardCount = hazardCount;

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
                    basicAttackTime3 = 3.5f;
                    basicAttackTime1 = 3.5f;
                    if (bossSpell==1 || bossSpell == 3 || bossSpell == 5 || bossSpell == 7)
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
                    if (bossSpell == 2) {
                        Instantiate(MistyEffect);
                    }else if (bossSpell == 4)
                    {
                        Instantiate(MistyEffect2);
                    }else if (bossSpell == 6)
                    {
                        //Instantiate(MistyEffect3);
                    }
                    robotImage.enabled = false;
                    break;
                }



                
                if (bossSpell == 8)
                {
                    int genRndBalls = 5;
                    if (i % genRndBalls == 0)
                    {
                        source.PlayOneShot(horn, 0.8f);
                        if (basicAttackTime3 > 0.75f)
                        {
                            float attackRatio = (float)bosslife / (float)bossBasicLife;
                            basicAttackTime3 = 3.5f * attackRatio;
                        }
                        if (basicAttackTime3 < 0.75f)
                        {
                            basicAttackTime3 = 0.75f;
                        }
                        genRndBalls = (int)Random.Range(3,8);
                        yield return new WaitForSeconds(basicAttackTime3);
                    }
                    position_x = Random.Range(-10, 10);
                    spawnPosition = new Vector3(position_x, 1.0f, 15.0f);
                    spawnRotation = Quaternion.identity;
                    Instantiate(hazard8, spawnPosition, spawnRotation);
                }
                /**
                *   Boss Spell Card: Big annoy ball
                *
                */
                else if (bossSpell == 7)
                {
                    if (i % 5 == 0)
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

                else if (bossSpell == 6)
                {
                    if (i % 14 == 0)
                    {

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

                        if (basicAttackTime3 > 0.15f)
                        {
                            //spell1_time -= (i/14)*0.1f;
                            float attackRatio = (float)bosslife / (float)bossBasicLife;
                            basicAttackTime3 = 2.5f * attackRatio;
                        }
                        if (basicAttackTime3 < 0.15f)
                        {
                            basicAttackTime3 = 0.15f;
                        }
                        yield return new WaitForSeconds(basicAttackTime3);
                        source.PlayOneShot(bossAttack2);

                    }

                    if (i % 14 <= random_skip && i % 14 >= random_skip - 1)
                    {
                        //Debug.Log("should have this in time increase " + i/14);
                        continue;
                    }

                    position_x = -7 + i % 14;
                    position_z = 25;
                    spawnPosition = new Vector3(position_x, 0.5f, position_z);
                    spawnRotation = Quaternion.identity;
                    Instantiate(hazard7, spawnPosition, spawnRotation);
                }

                /*
                 *  This is the normal attack from the boss (before spell card 1)
                 *  which sends a basic rock pattern and player just need to move left and right to dodge
                 *  
                 */

                else if (bossSpell == 5)
                {

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
                /**
                 *  Another basic attack with misty effect
                 * 
                 */
                else if (bossSpell == 4)
                {
                    //random_skip = 4 + (i/14)%6;

                    if (i % 14 == 0)
                    {

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

                        if (basicAttackTime2 > 0.15f)
                        {
                            //spell1_time -= (i/14)*0.1f;
                            float attackRatio = (float)bosslife / (float)bossBasicLife;
                            basicAttackTime2 = 2.5f * attackRatio;
                        }
                        if (basicAttackTime2 < 0.15f)
                        {
                            basicAttackTime2 = 0.15f;
                        }
                        yield return new WaitForSeconds(basicAttackTime2);
                        source.PlayOneShot(bossAttack2);

                    }

                    if (i % 14 <= random_skip && i % 14 >= random_skip - 1)
                    {
                        //Debug.Log("should have this in time increase " + i/14);
                        continue;
                    }

                    position_x = -7 + i % 14;
                    position_z = 25;
                    spawnPosition = new Vector3(position_x, 0.5f, position_z);
                    spawnRotation = Quaternion.identity;
                    Instantiate(hazard6, spawnPosition, spawnRotation);
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

                    if (i % 14 == 0)
                    {

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

                    if (i % 14 <= random_skip && i % 14 >= random_skip - 1)
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
                    if (i % 25 == 0)
                    {
                        if (spell3_time > 1f)
                        {
                            float attackRatio = (float)bosslife / (float)bossFinalLife;
                            spell3_time = 3.5f * attackRatio;
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
            score += 30000;
            if (!useProtection && !playerDeathInThisStage)
            {
                score += 30000;
            }
            useProtection = false;
            playerDeathInThisStage = false;
            updateScoreText();
            updateBonusScoreText();
            setBossExplode(true);
            bossSpell--;
            if (bossSpell == 1)
            {
                bosslife = bossFinalLife;
            }
            source.PlayOneShot(bossDefeated, 0.4f);
            if (bossSpell==0){
                bosslife = 0;
                score += 100000;
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
        Destroy(player);
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

    public bool getTriggerPlayerDeath()
    {
        return triggerPlayerDeath;
    }

    public bool getProtectionActive()
    {
        return protectionActive;
    }


    private float calculateBossHealthRatio() {
        if (bossSpell == 1)
        {
            return (float)bosslife / (float)bossFinalLife;
        }
        else
        {
            return (float)bosslife / (float)bossBasicLife;
        }
    }

    private void updateSpellCardText() {
        if (bossSpell == 8) {
            SpellCardName.text = "Beginning of the Star Road";
        }
        else if (bossSpell == 7) {
            SpellCardName.text = "[Earth Spell] Natural Mosaic";
        }
        else if (bossSpell == 6)
        {
            SpellCardName.text = "Believer's Narrow Road";
        } else if (bossSpell == 5)
        {
            SpellCardName.text = "[Earth Spell] The Great Refraction Wall";
        }
        else if (bossSpell == 4)
        {
            SpellCardName.text = "Fogged Reality";
        }
        else if (bossSpell == 3)
        {
            SpellCardName.text = "[Earth Spell] Rockshark";
        }
        else if (bossSpell == 2)
        {
            SpellCardName.text = "Misty Cosmic Path";
        }
        else if (bossSpell == 1)
        {
            SpellCardName.text = "[Thunder Spell] Particle Overflow";
        }
    }

    private void updateScoreText()
    { 
        playerScore.text = "Score: " + score;
    }
    private void updateBonusScoreText()
    {
        if(useProtection || playerDeathInThisStage)
            bonusScore.text = "current stage bonus: FAIL";
        else
            bonusScore.text = "current stage bonus: ON";
    }

    private void updatePlayerLifeText()
    {
        playerLifeText.text = "Player Life: " + playerLife;
    }
    public int getCurrentScore()
    {
        return score;
    }
}
