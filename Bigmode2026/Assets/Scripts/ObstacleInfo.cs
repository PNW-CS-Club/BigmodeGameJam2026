using UnityEngine;

public class ObstacleInfo : MonoBehaviour
{
    public GameObject prefab;
    public ObstacleType type;
}

public enum ObstacleType
{
    Basketball, SmallSquare, SmallTriangle, LockerVert, LockerHoriz
}