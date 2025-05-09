using UnityEngine;

public class GameScene : Player
{
    public GameObject GameCanvas;

    private void Start()
    {
        GameCanvas.SetActive(true);
    }

    public void OnRunButton()
    {
        GameCanvas.SetActive(false);
        Player.animator.SetBool("IsRun", true);
    }
}
