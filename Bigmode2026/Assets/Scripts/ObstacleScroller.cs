using UnityEngine;

public class ObstacleScroller : MonoBehaviour
{
    public float scrollSpeed = 1f;
    public float despawnYPos;
    private float distanceTraveled = 0; 
    
    private void Update() 
    {
        float distanceDelta = Time.deltaTime * scrollSpeed;
        distanceTraveled += distanceDelta;
        
        foreach (Transform child in transform) 
        {
            if (!child.gameObject.activeInHierarchy || (!child.CompareTag("Obstacle") && !child.CompareTag("Collectable")) ) continue;
            
            child.Translate(Vector2.down * distanceDelta, Space.World);

            if (child.position.y < despawnYPos) {
                Destroy(child.gameObject);
            }
        }
    }
}
