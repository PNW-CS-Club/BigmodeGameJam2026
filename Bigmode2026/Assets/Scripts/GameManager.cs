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

        // Generate obstacles
        // Note: Can use AnimationCurve in the future if we want to be fancy
        spawnInterval = Mathf.Max(0.4f, 2.0f - runTimer.runTime * 0.01f); // obstacles per second
        
        if(timeSinceLastObstacle >= spawnInterval){
            // Generate an obstacle
            GenerateObstacles();
            timeSinceLastObstacle = 0f; // reset timer
        }
    }

    private void StartRun(){
        // Set up Obstacle Spawn Timer
        timeSinceLastObstacle = 0f;
        runTimer.StartRun(); // runTimer.isRunning -> true

    }

    // currently never returns collectable
    public ObstacleDifficulty PickObstacleDifficulty(float t)
    {
        if (t < 4f) return ObstacleDifficulty.Collectable;
        
        if (t < 20f) return ObstacleDifficulty.Easy;

        if (t < 45f)
            return Random.value < 0.7f ? ObstacleDifficulty.Easy : ObstacleDifficulty.Medium;

        float r = Random.value;
        if (r < 0.5f) return ObstacleDifficulty.Medium;
        if (r < 0.85f) return ObstacleDifficulty.Hard;
        return ObstacleDifficulty.Easy;
    }

    private void GenerateObstacles()
    {
        //TODO: Choose an obstacle pool to spawn from
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
            case ObstacleDifficulty.Collectable: default:
                arr = collectables;
                break;
        }
        
        // Spawning obstacles 
        var info = arr[Random.Range(0, arr.Length)];
        GameObject obstacle = Instantiate(info.prefab, transform);

        if (difficulty == ObstacleDifficulty.Collectable) {
            obstacle.GetComponent<Collectable>().gameManager = this;
        }

        // Give the obstacle a random start position from the top
        obstacle.transform.position = new Vector3(Random.Range(-10.0f,10.0f), 7f, 0);


        // Give the obstacle a random initial velocity
        Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
        int direction = Random.Range(0,3);
        float obstacleSpeed = Random.Range(0f,5f);
        if(rb != null){
            if(direction == 0){
                // Set the velocity left
                rb.linearVelocity = Vector3.left * obstacleSpeed;
            } else if (direction == 1){
                // Set the velocity right
                rb.linearVelocity = Vector3.right * obstacleSpeed;
            } else{
                // Set the velocity straight down
                rb.linearVelocity = Vector3.down * obstacleSpeed;
            } 
        }
    }
}
