using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    public int score = 0;

    void Start()
    {
        scoreText.text = score.ToString() + " POINTS";
    }

    public void addPoints()
    {
        score += 10;
        scoreText.text = score.ToString() + " POINTS";
    }
}
