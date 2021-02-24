using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private int score;
    public Text UIScore;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        UIScore.text = "SCORE: 0";
    }

    public void IncrementScore()
    {
        score += 100;
        UIScore.text = "SCORE: " + score;
    }

    public void IncrememntScore(int score)
    {
        this.score += score;
        UIScore.text = "SCORE: " + score;
    }
}
