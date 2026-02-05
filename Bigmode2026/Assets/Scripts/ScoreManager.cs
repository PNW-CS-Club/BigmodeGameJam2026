using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    
    // can only be set from inside this class
    public int Score { get; private set; } = 0; 
    
    void Start() {
        scoreText.text = "POINTS: " + Score;
    }

    public void AddPoints(int points)
    {
        Score += points;
        scoreText.text = "POINTS: " + Score;
    }
}
