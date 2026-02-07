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
    [SerializeField] private PlayerController player;
    
    private RunTimer runTimer;

    private GameObject gameEndCanvas;

    private bool doingEndAnimation;
    private float endAnimationTimer;
    [SerializeField, Min(0.01f)] private float endAnimationDuration;

    // Obstacle variables
    private float spawnInterval; // The rate at which obstacles are spawned  (objects/second)
    private float timeSinceLastObstacle; // The Time.deltaTime since the last obstacle was spawned
    private float timeSinceLastCollectable; // The Time.deltaTime since the last collectable was spawned

    [SerializeField] private float pointsPerSecond = 5f;
    public float Score { get; private set; } = 0f;

    void Awake() {
        runTimer = GetComponent<RunTimer>();    
        gameEndCanvas = GameObject.Find("GameEndCanvas");
    }
    
    void Start() {
        StartRun();
    }

    void Update()
    {
        if (doingEndAnimation) {
            endAnimationTimer += Time.unscaledDeltaTime;
            
            if (endAnimationTimer < endAnimationDuration) {
                Time.timeScale = 1f - (endAnimationTimer / endAnimationDuration);
            }
            else {
                Time.timeScale = 0f;
                doingEndAnimation = false;
                // make end menu appear here
                var endMenu = gameEndCanvas.GetComponent<EndMenu>();
                endMenu.ActivateMenu();
            }

            return;
        }
        
        if(runTimer == null) return;
        if(!runTimer.isRunning) return; // game paused or game ended

        timeSinceLastObstacle += Time.deltaTime; // increment with time
        timeSinceLastCollectable += Time.deltaTime;

        // Generate obstacles
        spawnInterval = Mathf.Max(0.3f, 1.5f - runTimer.runTime * 0.02f); // obstacles per second
        
        if(timeSinceLastObstacle >= spawnInterval){
            // Generate an obstacle
            GenerateObstacle();
            timeSinceLastObstacle = 0f; // reset timer
        }
        // Generate an obstacle every second
        if(timeSinceLastCollectable >= 1f){
            GenerateCollectables();
            timeSinceLastCollectable = 0f; // reset timer
        }
        
        // Add points and update score text constantly
        Score += Time.deltaTime * pointsPerSecond;
        scoreText.text = "POINTS: " + (int)Score;
    }

    public void ResetRun() 
    {
        foreach (Transform child in transform) {
            if (child.CompareTag("Obstacle")) {
                Destroy(child.gameObject);
            }
        }
        
        StartRun();
    }

    private void StartRun() {
        // Reset timers
        timeSinceLastObstacle = 0f;
        timeSinceLastCollectable = 0f;
        // Reset Score
        Score = 0;
        scoreText.text = "POINTS: " + (int)Score;
        // Start a new run
        runTimer.StartRun(); // runTimer.isRunning -> true

        player.ResetPlayer();
    }

    private void EndRun()
    {
        doingEndAnimation = true;
        endAnimationTimer = 0f;
        runTimer.EndRun();
        if( Score >= HighScore.Instance.GetScore()){
            HighScore.Instance.SetScore(Score);
            Debug.Log("New High Score: " + HighScore.Instance.GetScore());
        }
    }

    private ObstacleDifficulty PickObstacleDifficulty(float t)
    {
        if (t < 5f) return ObstacleDifficulty.Easy;
        
        if (t < 15f) return Random.value < 0.7f ? ObstacleDifficulty.Easy : ObstacleDifficulty.Medium;
        
        float r = Random.value;
        if (r < 0.4f) return ObstacleDifficulty.Medium;
        if (r < 0.6f) return ObstacleDifficulty.Hard;
        return ObstacleDifficulty.Easy;
    }

    public void AddPoints(float points) => Score += points;

    private void GenerateObstacle()
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
        float xPos = Mathf.Round(Random.Range(-11.0f + obstacleLength/2f, 11.0f - obstacleLength/2f));
        // Round the y position to the nearest unit in relation to the floor

        obstacle.transform.position = new Vector3(xPos, 10f, 0);

        if (info.isProjectile) {
            Rigidbody2D rb = obstacle.GetComponent<Rigidbody2D>();
            if (rb == null) {
                Debug.LogWarning($"Failed to throw obstacle {info.prefab.name} without rigidbody");
                return;
            }
            
            // Give the obstacle a random initial velocity
            float obstacleSpeed = Random.Range(3f,7f);
            float angle = Random.Range(-60f, 60f) * Mathf.Deg2Rad; 
            Vector2 direction = Mathf.Sin(angle) * Vector2.right + Mathf.Cos(angle) * Vector2.down; 
            
            rb.AddForce(direction * obstacleSpeed, ForceMode2D.Impulse);
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
