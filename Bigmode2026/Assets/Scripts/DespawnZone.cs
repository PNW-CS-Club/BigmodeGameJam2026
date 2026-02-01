using UnityEngine;
using UnityEngine.Pool;

public class DespawnZone : MonoBehaviour
{
    [SerializeField] PoolManager poolManager;
    
    void OnTriggerEnter2D(Collider2D other){
        GameObject obj = other.transform.parent.gameObject;
        if(other.CompareTag("Floor")){
            poolManager.OnRelease(obj);
        }
    }
}
