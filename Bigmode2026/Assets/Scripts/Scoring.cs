using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static int points = 0;

    //time is based on the score
    [SerializeField] 
    private GameObject CollectableObject;

    public bool collected = false; 

    private Vector3 pos;

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collected)
        {
            points += 10;
            Debug.Log("You collected the start" + points);
            CollectableObject.SetActive(false);
        }
    }

    void Start()
    {
        spawnInObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnInObject()
    {
        // Gives object a random postion
        pos = new Vector3(Random.Range(-10.0f,10.0f), 7f, 0);

        // Instantiate the collectable object
        Instantiate(CollectableObject, pos, Quaternion.identity);
    }
}
