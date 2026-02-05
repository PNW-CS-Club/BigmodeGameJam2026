using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawn : MonoBehaviour
{
    private RunTimer runTimer;
    public GameObject CoinPrefab;
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
            SpawnObject();
            timeSinceLastCoinSpawn = 0;
        }
    }

    void SpawnObject()
    {
        // Gives object a random position
        pos = new Vector3(Random.Range(-10.0f,10.0f), 7f, 0);

        // Instantiate the collectable object
        GameObject Coin = Instantiate(CoinPrefab, pos, Quaternion.identity);
        Collectable collectable = Coin.GetComponent<Collectable>();
        collectable.manager = gameObject.GetComponent<ScoreManager>();

        Rigidbody2D rb = CoinPrefab.GetComponent<Rigidbody2D>();
        
        float obstacleSpeed = Random.Range(0f,5f);
        rb.linearVelocityY = obstacleSpeed;
    }
}
