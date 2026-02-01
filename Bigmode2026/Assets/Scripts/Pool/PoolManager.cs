using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    
    //private List<PooledObstacle> pools;
    // Obstacle prefabs
    public GameObject smallSquare;
    public GameObject smallTriangle;
    public ObjectPool<GameObject> pool;
    //private Dictionary<ObstacleDifficulty, ObjectPool<GameObject>> poolMap;

    void Awake()
    {
        //poolMap = new Dictionary<ObstacleDifficulty, ObjectPool<GameObject>>();

        // Instantiate a pool for each obstacle pool
        // foreach(var entry in pools)
        // {
        //     var pool = new ObjectPool<GameObject>( ...
        //     );

        //     //poolMap.Add(entry.ObstacleDifficulty,pool);
        // }

        // Create a pool with the four core callbacks
        pool = new ObjectPool<GameObject>(
            createFunc: CreateObstacle,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyObstacle,
            collectionCheck: false, // Use to debug double release mistakes
            defaultCapacity: 10,
            maxSize: 30
        );

    }

    // Creates a new pooled GameObject the first time (and whenever the pool needs more)
    private GameObject CreateObstacle()
    {
        GameObject obj = Instantiate(smallSquare);
        obj.SetActive(false);
        return obj;
    }

    private void OnGet(GameObject obj)
    {
        obj.SetActive(true); 
    }

    private void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    // Called when pool decides to destroy an obstacle (above max size)
    private void OnDestroyObstacle(GameObject obj)
    {
        Destroy(obj);
    }
 

}
