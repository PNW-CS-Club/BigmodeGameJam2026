using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static int points = 0;

    //time is based on the score

    [SerializeField] 
    private GameObject ExtraCollectingObject;

    public bool collected = false; 

    public int min = -10;
    public int max = 10;

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !collected)
        {
            points += 10;
            Debug.Log("You collected the start" + points);
            ExtraCollectingObject.SetActive(false);
        }
    }

    void Start()
    {
        //whatever gameobject that we decided to use here
        spawnInObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void pointsTraveled()
    {
        if(points > 0)
        {
            
        }
    }

    void spawnInObject()
    {
        Vector3 spawnpoint = new Vector3(0,0,0);
        Instantiate(ExtraCollectingObject);

        ExtraCollectingObject.SetActive(true);
    }
}
