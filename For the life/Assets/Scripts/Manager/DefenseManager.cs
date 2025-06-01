using UnityEngine;

public class DefenseManager : MonoBehaviour
{
    public static DefenseManager instance; // 싱글톤 인스턴스

    public bool isTurning = false; // 턴이 진행 중인지 여부

    private void Start()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 현재 객체를 파괴
        }
    }
}
