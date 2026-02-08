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
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private PlayerController player;
    
    private RunTimer runTimer;

    // End menu variables
    private GameObject gameEndCanvas;
    private EndMenu endMenu;

    private bool doingEndAnimation;
    private float endAnimationTimer;
    [SerializeField, Min(0.01f)] private float endAnimationDuration;

    // Obstacle variables
    private float spawnInterval; // The rate at which obstacles are spawned  (objects/second)
    private float timeSinceLastObstacle; // The Time.deltaTime since the last obstacle was spawned
    private float timeSinceLastCollectable; // The Time.deltaTime since the last collectable was spawned

    [SerializeField] private float pointsPerSecond = 5f;
    public float Score { get; private set; } = 0f;
    //ublic float HScore {get; private set; } = 0f;

    void Awake() {
        runTimer = GetComponent<RunTimer>();    
        gameEndCanvas = GameObject.Find("GameEndCanvas");
        endMenu = gameEndCanvas.GetComponent<EndMenu>();
    }
    
    void Start() {
        StartRun();
    }

    private void StartTextSettings(){
        // set alignment
        scoreText.alignment = TextAlignmentOptions.Right;
        highScoreText.alignment = TextAlignmentOptions.Right;
        // set position
        scoreText.transform.localPosition = new Vector2(690,470);
        highScoreText.transform.localPosition = new Vector2(690, 520);
        // set color
        Color purple = new Color(44f / 255f, 27f / 255f, 46f / 255f);
        scoreText.color = purple;
        highScoreText.color = purple;
        // set font size
        scoreText.fontSize = 48;
        highScoreText.fontSize = 48;
    }
    
    private void EndTextSettings(){
        // set alignment
        scoreText.alignment = TextAlignmentOptions.Center;
        highScoreText.alignment = TextAlignmentOptions.Center;
        // set position
        scoreText.transform.localPosition = new Vector2(165,50);
        highScoreText.transform.localPosition = new Vector2(165, 145);
        // set color
        Color beige = new Color(255f / 255f, 244f / 255f, 224f / 255f);
        scoreText.color = beige;
        highScoreText.color = beige;
        // set font size
        scoreText.fontSize = 64f;
        highScoreText.fontSize = 64f;
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
    
                // make end menu appear
                endMenu.ActivateMenu();
                // Update the score text
                EndTextSettings();
                
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
        scoreText.text = "SCORE: " + (int)Score;
        // Update high score text too if score is greater than high score
        if(Score > HighScore.Instance.GetScore()){
            highScoreText.text = "BEST: " + (int)Score;
        }
    }

    public void ResetRun() 
    {
        foreach (Transform child in transform) {
            if (child.CompareTag("Obstacle") || child.CompareTag("Collectable")) {
                Destroy(child.gameObject);
            }
        }
        StartTextSettings();
        StartRun();
    }

    private void StartRun() {
        // Reset timers
        timeSinceLastObstacle = 0f;
        timeSinceLastCollectable = 0f;
        // Reset Score
        Score = 0;
        scoreText.text = "SCORE: " + (int)Score;
        highScoreText.text = "BEST: " + (int)HighScore.Instance.GetScore();
        // Start a new run
        runTimer.StartRun(); // runTimer.isRunning -> true

        player.ResetPlayer();
        Time.timeScale = 1f;
    }

    private void EndRun()
    {
        doingEndAnimation = true;
        endAnimationTimer = 0f;
        runTimer.EndRun();
        if( Score >= HighScore.Instance.GetScore()){
            HighScore.Instance.SetScore(Score);
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
