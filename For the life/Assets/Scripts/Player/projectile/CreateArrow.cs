using UnityEngine;

public class CreateArrow : MonoBehaviour
{
    public GameObject arrowPrefab; //화살 프리팹
    public Transform arrowSpawnPoint_Middle; //화살이 생성될 위치
    public Transform arrowSpawnPoint_Top; //화살이 생성될 위치
    public Transform arrowSpawnPoint_Bottom; //화살이 생성될 위치

    //화살 생성 함수
    public void CreateMiddle()
    {
        if (arrowPrefab != null && arrowSpawnPoint_Middle != null)
        {
            //화살 프리팹을 지정된 위치에 생성
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint_Middle.position, arrowSpawnPoint_Middle.rotation);
            //화살이 생성된 후 추가적인 설정이 필요하다면 여기에 작성
            arrow.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Arrow prefab or spawn point is not assigned.");
        }
    }

    public void CreateTop()
    {
        if (arrowPrefab != null && arrowSpawnPoint_Top != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint_Top.position, arrowSpawnPoint_Top.rotation);
            arrow.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Arrow prefab or spawn point is not assigned.");
        }
    }

    public void CreateBottom()
    {
        if (arrowPrefab != null && arrowSpawnPoint_Bottom != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint_Bottom.position, arrowSpawnPoint_Bottom.rotation);
            arrow.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Arrow prefab or spawn point is not assigned.");
        }
    }
}
