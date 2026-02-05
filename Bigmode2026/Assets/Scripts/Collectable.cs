using UnityEngine;

public class Collectable : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player")) return;
        
        gameManager.AddPoints(10);
        Debug.Log("You collected the coin");
        gameObject.SetActive(false);
    }
}
