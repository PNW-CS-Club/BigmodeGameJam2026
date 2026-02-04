using UnityEngine;

public class ObstacleInfo : MonoBehaviour
{
    public GameObject prefab;
    public ObstacleType type;
    public float unitLength; // The length of the obstacle 
}

public enum ObstacleType
{
    Basketball, SmallSquare, SmallTriangle, LockerVert, LockerHoriz
}