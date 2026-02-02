using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    [SerializeField]  private ObstacleInfo[] obstacleInfoList;
    [HideInInspector] public  Dictionary<ObstacleType, ObjectPool<GameObject>> poolMap;

    void Awake()
    {
        poolMap = new();

        foreach (var obstacleInfo in obstacleInfoList) {
            var prefab = obstacleInfo.prefab;
            
            ObjectPool<GameObject> pool = new (
                createFunc: () => CreateObstacle(prefab), // captures the reference to "prefab"
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                actionOnDestroy: OnDestroyObstacle,
                collectionCheck: false, // Use to debug double release mistakes
                defaultCapacity: 10,
                maxSize: 30
            );
            
            poolMap.Add(obstacleInfo.type, pool);
        }

    }

    // Creates a new pooled GameObject the first time (and whenever the pool needs more)
    private GameObject CreateObstacle(GameObject obstacle)
    {
        GameObject obj = Instantiate(obstacle, transform);
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


    public GameObject GetObstacle(ObstacleType type) {
        if (poolMap.TryGetValue(type, out var pool)) {
            return pool.Get();
        }
        else {
            throw new KeyNotFoundException($"Failed to find obstacle pool of type {type}");
        }
    }
    
    
    public void TryRelease(GameObject obj) {
        var info = obj.GetComponent<ObstacleInfo>();
        if (!info) return;

        if (!poolMap.TryGetValue(info.type, out var pool)) return;
        pool.Release(obj);
    }

}
