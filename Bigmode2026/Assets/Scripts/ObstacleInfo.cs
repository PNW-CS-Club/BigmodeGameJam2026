using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle Info", menuName = "ScriptableObjects/ObstacleInfo")]
public class ObstacleInfo : ScriptableObject
{
    public GameObject prefab;
    public float unitLength;
}

public enum ObstacleType
{
    Basketball, LockerVert, LockerHoriz, Coin, Desk, WetFloorSign
}

public enum ObstacleDifficulty
{
    Easy, Medium, Hard, Collectable
}