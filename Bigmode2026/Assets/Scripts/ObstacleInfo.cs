using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle Info", menuName = "ScriptableObjects/ObstacleInfo")]
public class ObstacleInfo : ScriptableObject
{
    public GameObject prefab;
    public float unitLength;
    public bool isProjectile;
}

public enum ObstacleDifficulty
{
    Easy, Medium, Hard, Collectable
}