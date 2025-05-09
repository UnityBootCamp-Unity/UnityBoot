using System.Collections;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public Player player;
    public EndManager endManager;
    public GameScene gameCanvas;

    private bool isTime = false;

    private void Start()
    {
    }
    private void Update()
    {
        if (scoreManager.max_score == scoreManager.score)
        {
            endManager.gameObject.SetActive (true);
            scoreManager.gameObject.SetActive (false);
            player.gameObject.SetActive (false);
        }


        if (scoreManager.current_time >= scoreManager.end_time)
        {
            endManager.gameObject.SetActive(true);
            scoreManager.gameObject.SetActive(false);
            player.gameObject.SetActive(false);
        }

        if (isTime == false && gameCanvas.gameObject.activeSelf == false)
        {
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        isTime = true;
        yield return new WaitForSeconds(1f);
        scoreManager.UpTime();
        isTime = false;
    }
}
