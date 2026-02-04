using System.Collections.Generic;
using UnityEngine;

public enum ObstacleDifficulty
{
    Easy,
    Medium,
    Hard
}


public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PoolManager poolManager;

    [Header("Obstacle Lists")]
    [SerializeField] private List<ObstacleInfo> easyObstacles;
    [SerializeField] private List<ObstacleInfo> mediumObstacles;
    [SerializeField] private List<ObstacleInfo> hardObstacles;

    private RunTimer runTimer; 

    // Obstacle variables
    private Vector3 pos; // Position of the obstacle
    private Quaternion rot; // Rotation of the obstacle
    private float angleDegrees; // Angle of the obstacle in degrees

    private float spawnInterval; // The rate at which obstacles are spawned  (objects/second)
    private float timeSinceLastObstacle; // The Time.deltaTime since the last obstacle was spawned

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set up Run Timer
        runTimer = GetComponent<RunTimer>();
        
        StartRun();
    }

    // Update is called once per frame
    void Update()
    {
        if(runTimer == null) return;
        if(!runTimer.isRunning) return; // game paused or game ended

        timeSinceLastObstacle += Time.deltaTime; // increment with time

        // Generate obstacles
        // Note: Can use AnimationCurve in the future if we want to be fancy
        spawnInterval = Mathf.Max(0.4f, 2.0f - runTimer.runTime * 0.01f); // obstacles per second
        
        if(timeSinceLastObstacle >= spawnInterval){
            // Generate an obstacle
            GenerateObstacles();
            timeSinceLastObstacle = 0f; // reset timer
        }
    }

    /** FixedUpdate is called at fixed time intervals, synchronized with the physics system.
    *   Use for physics related calculations
    */
    void FixedUpdate()
    {
        
    }

    private void StartRun(){
        // Set up Obstacle Spawn Timer
        timeSinceLastObstacle = 0f;
        runTimer.StartRun(); // runTimer.isRunning -> true

    }

    public ObstacleDifficulty PickObstacleDifficulty(float t)
    {
        // if (t < 10f) return ObstacleDifficulty.Easy;

        // if (t < 25f)
        //     return Random.value < 0.7f ? ObstacleDifficulty.Easy : ObstacleDifficulty.Medium;
        
        // float r = Random.value;
        // if (r < 0.5f) return ObstacleDifficulty.Medium; // 50%
        // if (r < 0.85f) return ObstacleDifficulty.Hard;  // 35%
        // return ObstacleDifficulty.Easy;                 // 15%

        if (t < 5f) return ObstacleDifficulty.Easy;

        if (t < 10f)
            return Random.value < 0.7f ? ObstacleDifficulty.Easy : ObstacleDifficulty.Medium;
        
        float r = Random.value;
        if (r < 0.5f) return ObstacleDifficulty.Medium; // 50%
        if (r < 0.85f) return ObstacleDifficulty.Hard;  // 35%
        return ObstacleDifficulty.Easy;                 // 15%
    }

    private void GenerateObstacles()
    {
        // Choose an obstacle to spawn
        ObstacleDifficulty difficulty = PickObstacleDifficulty(runTimer.runTime);
        ObstacleType obstacleType;
        switch(difficulty)
        {
            case ObstacleDifficulty.Easy:
                int i = Random.Range(0, easyObstacles.Count);
                obstacleType = easyObstacles[i].type; // Get a random easy obstacle
                break;
            case ObstacleDifficulty.Medium:
                i = Random.Range(0, mediumObstacles.Count);
                obstacleType = mediumObstacles[i].type; // Get a random easy obstacle
                break;
            case ObstacleDifficulty.Hard:
                i = Random.Range(0, hardObstacles.Count);
                obstacleType = hardObstacles[i].type; // Get a random easy obstacle
                break;
            default: 
                i = Random.Range(0, easyObstacles.Count);
                obstacleType = easyObstacles[i].type; // Get a random easy obstacle
                break;
            
        }
        
        // Spawning obstacles 
        GameObject obstacle = poolManager.GetObstacle(obstacleType);

        // Give the obstacle a random start position from the top
        float obstacleLength = obstacle.GetComponent<ObstacleInfo>().unitLength;
        // Round the x position to the nearest unit / Spawn object at y = 10 / z = 0
        obstacle.transform.position = new Vector3(Mathf.Round(Random.Range(-11.0f+obstacleLength/2f, 11.0f-obstacleLength/2f)), 10f, 0);
        


        // Give the obstacle a random initial velocity
        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
        int direction = Random.Range(0,3);
        float obstacleSpeed = Random.Range(3f,7f);
        if(rb != null){
            if(direction == 0){
                // Set the velocity left
                rb.linearVelocity += Vector2.left * obstacleSpeed;
                rb.linearVelocity += Vector2.down * obstacleSpeed;
            } else if (direction == 1){
                // Set the velocity right
                rb.linearVelocity += Vector2.right * obstacleSpeed;
                rb.linearVelocity += Vector2.down * obstacleSpeed;
            } else{
                // Set the velocity straight down
                rb.linearVelocity += Vector2.down * obstacleSpeed;
            } 
        }
    }
}
