using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTimeAndExplosion : MonoBehaviour
{
    public AudioClip explosionSound;
    public float lifetime;
    public GameObject explosion;

    private int lifeCount = 0;

    private void Update()
    {
        if (lifeCount > 100)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        lifeCount++;
    }
}
