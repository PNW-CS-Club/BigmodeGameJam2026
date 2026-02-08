using UnityEngine;

public class HighScore : MonoBehaviour
{
    public static HighScore Instance {get; set;}

    private float Score = 0f;

    void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetScore(float score){
        Score = score;
    }

    public float GetScore(){
        return Score;
    }

   
}
