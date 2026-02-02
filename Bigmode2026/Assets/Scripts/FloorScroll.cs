using UnityEngine;

public class FloorScroll : MonoBehaviour
{
    public float scrollSpeed = 1f;
    public float wrapDistance;
    private float distanceTraveled = 0;
    
    void Update() {
        float distanceDelta = scrollSpeed * Time.deltaTime; 
        distanceTraveled += distanceDelta;
        transform.Translate(Vector2.down * distanceDelta);

        if (distanceTraveled > wrapDistance) {
            distanceTraveled -= wrapDistance;
            transform.Translate(Vector2.up * wrapDistance);
        }
    }
}
