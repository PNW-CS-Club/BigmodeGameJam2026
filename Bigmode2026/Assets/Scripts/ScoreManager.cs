using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int score = 0;
    void Start()
    {
        
        scoreText.text = "POINTS: " + score.ToString();
    }

    public void addPoints()
    {
        score += 10;
        scoreText.text = "POINTS: " + score.ToString();;
    }
}
