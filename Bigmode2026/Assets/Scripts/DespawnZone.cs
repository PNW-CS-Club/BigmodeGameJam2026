using UnityEngine;
using UnityEngine.Pool;

public class DespawnZone : MonoBehaviour
{
    [SerializeField] PoolManager poolManager;
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Obstacle")){
            poolManager.pool.Release(other.gameObject);
        }
    }
}
