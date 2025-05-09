using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    public int max_score = 20;

    public int current_time = 0;

    public int end_time = 65;

    public TMP_Text scoreText;
    public TMP_Text timeText;

    private void Start()
    {
        SetTMP_Text();
    }

    private void Update()
    {
        SetTMP_Text();
    }

    public void SetTMP_Text()
    {
        scoreText.text = $"TotalScore {max_score} : {score}";
        timeText.text = $"Limit {end_time} : {current_time}";
    }

    public void UpScore()
    {
        score++;
        SetTMP_Text();
    }

    public void UpTime()
    {
        current_time ++;
        SetTMP_Text();
    }
}
