using UnityEngine;

public class Collectable : MonoBehaviour
{
    public ScoreManager manager;

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player")) return;
        
        manager.AddPoints(10);
        Debug.Log("You collected the coin");
        gameObject.SetActive(false);
    }
}
