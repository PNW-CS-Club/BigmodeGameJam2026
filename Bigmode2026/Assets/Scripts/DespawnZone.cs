using UnityEngine;
using UnityEngine.Pool;

public class DespawnZone : MonoBehaviour
{
    private ObjectPool<GameObject> pool;
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Floor")){
            pool.Release(gameObject);
        }
    }
}
