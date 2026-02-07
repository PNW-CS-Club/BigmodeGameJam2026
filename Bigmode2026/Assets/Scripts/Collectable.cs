using UnityEngine;

public class Collectable : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player")) return;
        SoundManager.Instance.PlaySound3D("Coins", transform.position);
        gameManager.AddPoints(10);
        Destroy(gameObject);
    }
}
