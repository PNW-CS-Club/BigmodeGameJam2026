using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    private RunTimer runTimer;
    public GameObject Coin;
    private Vector3 pos;
    private float timeSinceLastCoinSpawn;
    private float spawnInterval;

    void Update()
    {
        // if(runTimer == null) return;
        // if(!runTimer.isRunning) return; // game paused or game ended

        timeSinceLastCoinSpawn += Time.deltaTime; //incrementing with time

        spawnInterval = Random.Range(5f, 10f); //random spawn time

        if(timeSinceLastCoinSpawn >= spawnInterval)
        {
            //spawn in the bonus collectable
            spawnInObject();
            timeSinceLastCoinSpawn = 0;
        }
    }

    void spawnInObject()
    {
        // Gives object a random postion
        pos = new Vector3(Random.Range(-10.0f,10.0f), 7f, 0);

        // Instantiate the collectable object
        Instantiate(Coin, pos, Quaternion.identity);

        Rigidbody2D rb = Coin.GetComponent<Rigidbody2D>();
        
        float obstacleSpeed = Random.Range(0f,5f);
        rb.linearVelocityY = obstacleSpeed;
    }
}
