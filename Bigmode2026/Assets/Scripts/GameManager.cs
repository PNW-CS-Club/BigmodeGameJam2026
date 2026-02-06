using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [Header("References")]
    
    [SerializeField]  private ObstacleInfo[] easyObstacles;
    [SerializeField]  private ObstacleInfo[] mediumObstacles;
    [SerializeField]  private ObstacleInfo[] hardObstacles;
    [SerializeField]  private ObstacleInfo[] collectables;
    
    [SerializeField] private TMP_Text scoreText;
    
    private RunTimer runTimer; 

    // Obstacle variables
    private Vector3 pos; // Position of the obstacle
    private Quaternion rot; // Rotation of the obstacle
    private float angleDegrees; // Angle of the obstacle in degrees

    private float spawnInterval; // The rate at which obstacles are spawned  (objects/second)
    private float timeSinceLastObstacle; // The Time.deltaTime since the last obstacle was spawned
    private float timeSinceLastCollectable; // The Time.deltaTime since the last collectable was spawned

    public int Score { get; private set; } = 0; 
    
    void Start() {
        scoreText.text = "POINTS: " + Score;
        
        // Set up Run Timer
        runTimer = GetComponent<RunTimer>();
        
        StartRun();
    }
    
    public void AddPoints(int points)
    {
        Score += points;
        scoreText.text = "POINTS: " + Score;
    }

    void Update()
    {
        if(runTimer == null) return;
        if(!runTimer.isRunning) return; // game paused or game ended

        timeSinceLastObstacle += Time.deltaTime; // increment with time
        timeSinceLastCollectable += Time.deltaTime;

        // Generate obstacles
        spawnInterval = Mathf.Max(0.4f, 2.0f - runTimer.runTime * 0.01f); // obstacles per second
        
        if(timeSinceLastObstacle >= spawnInterval){
            // Generate an obstacle
            GenerateObstacles();
            timeSinceLastObstacle = 0f; // reset timer
        }
        // Generate an obstacle every second
        if(timeSinceLastCollectable >= 1f){
            GenerateCollectables();
            // Add points every second
            AddPoints(5);
            timeSinceLastCollectable = 0f; // reset timer
        }
    }

    private void StartRun(){
        // Set up Obstacle Spawn Timer
        timeSinceLastObstacle = 0f;
        timeSinceLastCollectable = 0f;
        Score = 0;
        runTimer.StartRun(); // runTimer.isRunning -> true

    }

    public ObstacleDifficulty PickObstacleDifficulty(float t)
    {
        if (t < 5f) return ObstacleDifficulty.Easy;
        
        if (t < 10f) return ObstacleDifficulty.Medium;

        float r = Random.value;
        if (r < 0.5f) return ObstacleDifficulty.Medium;
        if (r < 0.85f) return ObstacleDifficulty.Hard;
        return ObstacleDifficulty.Easy;
    }

    private void GenerateObstacles()
    {
        // Choose an obstacle pool to spawn from
        var difficulty = PickObstacleDifficulty(runTimer.runTime);
        ObstacleInfo[] arr;
        
        switch (difficulty) {
            case ObstacleDifficulty.Easy:
                arr = easyObstacles;
                break;
            case ObstacleDifficulty.Medium:
                arr = mediumObstacles;
                break;
            case ObstacleDifficulty.Hard:
                arr = hardObstacles;
                break;
            default:
                arr = easyObstacles;
                break;
        }
        
        // Spawning obstacles 
        var info = arr[Random.Range(0, arr.Length)];
        GameObject obstacle = Instantiate(info.prefab, transform);

        // Give the obstacle a random start position from the top
        float obstacleLength = info.unitLength;
        // Round the x position to the nearest unit / Spawn object at y = 10 / z = 0
        float xPos = Mathf.Round(Random.Range(-11.0f+obstacleLength/2f, 11.0f-obstacleLength/2f));
        // Round the y position to the nearest unit in relation to the floor

        obstacle.transform.position = new Vector3(xPos, 10f, 0);


        // Give the obstacle a random initial velocity
        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
        int direction = Random.Range(0,3);
        float obstacleSpeed = Random.Range(3f,7f);
        if(rb != null){
            if(direction == 0){
                // Set the velocity left and down
                rb.linearVelocity += Vector2.left * obstacleSpeed;
                rb.linearVelocity += Vector2.down * obstacleSpeed;
            } else if (direction == 1){
                // Set the velocity right and down
                rb.linearVelocity += Vector2.right * obstacleSpeed;
                rb.linearVelocity += Vector2.down * obstacleSpeed;
            } else{
                // Set the velocity straight down
                rb.linearVelocity += Vector2.down * obstacleSpeed;
            } 
        }
    }

    private void GenerateCollectables(){
        var info = collectables[Random.Range(0, collectables.Length)];
        GameObject collectable = Instantiate(info.prefab, transform);

        collectable.GetComponent<Collectable>().gameManager = this;

        // Give the collectable a random start position from the top
        float collectableLength = info.unitLength;
        // Round the x position to the nearest unit / Spawn object at y = 10 / z = 0
        float xPos = Mathf.Round(Random.Range(-11.0f+collectableLength/2f, 11.0f-collectableLength/2f));
        // Round the y position to the nearest unit in relation to the floor

        collectable.transform.position = new Vector3(xPos, 10f, 0);
    }
}
