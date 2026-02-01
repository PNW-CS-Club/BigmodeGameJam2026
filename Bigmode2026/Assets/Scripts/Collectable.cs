using System.Drawing;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Collectable : MonoBehaviour
{
    ScoreManager manager;
    [SerializeField] GameObject Floor;
    public bool collected = false; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(true);

        if(collision.CompareTag("Player") && !collected)
        {
            manager.addPoints();
            Debug.Log("You collected the coin");
            gameObject.SetActive(false);
        }
        else if (collision == Floor)
        {
            Destroy(gameObject);
        }
    }
}
