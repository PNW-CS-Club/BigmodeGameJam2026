using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle Info", menuName = "ScriptableObjects/ObstacleInfo")]
public class ObstacleInfo : ScriptableObject
{
    public GameObject prefab;
    public ObstacleType type;
    public ObstacleDifficulty difficulty;
}

public enum ObstacleType
{
    Basketball, SmallSquare, SmallTriangle, LockerVert, LockerHoriz, Coin
}

public enum ObstacleDifficulty
{
    Easy, Medium, Hard, Collectable
}