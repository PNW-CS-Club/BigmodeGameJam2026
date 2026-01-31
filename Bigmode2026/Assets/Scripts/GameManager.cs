using UnityEngine;

public class GameManager : MonoBehaviour
{
    private RunTimer runTimer; 
    
    // Obstacle prefabs
    public GameObject smallSquare;

    // Obstacle variables
    public int numberOfObstacles = 0; // The current number of obstacles in the scene
    public int maxObstacles = 5; // The maximum number of obstacles allowed in the scene
    private Vector3 pos; // Position of the obstacle
    private Quaternion rot; // Rotation of the obstacle
    private Vector3 initVelocity; // Initial velocity of the obstacle
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
            Debug.Log(spawnInterval);
            timeSinceLastObstacle = 0f; // reset timer
        }

            
        
        


        //TODO: Delete obstacles that reach the bottom of the scene
        // Place a trigger below the visible screen that deactivates objects when they collide
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

    private void GenerateObstacles()
    {
        //Instantiate(smallSquare, new Vector3(0,7,0), Quaternion.identity); //test
        //TODO: Pick a random obstacle from a list

        //TODO: Give the obstacle a random start position from the top
        // x bounds: ( a , b )
        // y: 7
        // z: 0
        pos = transform.position + new Vector3(Random.Range(-10.0f,10.0f), 7f, 0);

        //TODO: Give the object a random rotation
        angleDegrees = 0f;
        rot = Quaternion.Euler(0, 0, angleDegrees);

        // Instantiate the obstacle
        if(numberOfObstacles < maxObstacles){
            Instantiate(smallSquare, pos, rot);
            ++numberOfObstacles;
        }

        

        
    }
}
