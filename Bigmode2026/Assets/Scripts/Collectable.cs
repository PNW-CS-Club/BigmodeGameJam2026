using System.Drawing;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Collectable : MonoBehaviour
{
    public ScoreManager manager;
    public bool collected = false; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collected)
        {   
            collected = true;
            manager.addPoints();
            Debug.Log("You collected the coin");
            gameObject.SetActive(false);
        }
    }
}
