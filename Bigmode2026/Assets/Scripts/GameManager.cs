using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Obstacle prefabs
    public GameObject smallSquare;

    // Obstacle variables
    public int numberOfObstacles = 0; // The current number of obstacles in the scene
    public int maxObstacles = 5; // The maximum number of obstacles allowed in the scene
    private Vector3 pos;
    private Quaternion rot;
    private float angleDegrees;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Delete obstacles that reach the bottom of the scene
        // Place a trigger below the visible screen that deactivates objects when they collide
    }

    /** FixedUpdate is called at fixed time intervals, synchronized with the physics system.
    *   Use for physics related calculations
    */
    void FixedUpdate()
    {
        
    }

    public void GenerateObstacles()
    {
        //Instantiate(smallSquare, new Vector3(0,7,0), Quaternion.identity); //test
        //TODO: Pick a random obstacle from a list

        //TODO: Give the obstacle a random start position from the top
        // x bounds: ( a , b )
        // y: 7
        // z: 0
        pos = transform.position + new Vector3(0, 7, 0);

        //TODO: Give the object a random rotation
        angleDegrees = 25f;
        rot = Quaternion.Euler(0, 0, angleDegrees);

        // Instantiate the obstacle
        Instantiate(smallSquare, pos, rot);

        

        
    }
}
