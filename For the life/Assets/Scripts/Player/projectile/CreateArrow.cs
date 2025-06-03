using UnityEngine;

public class CreateArrow : MonoBehaviour
{
    public GameObject arrowPrefab; //ȭ�� ������
    public Transform arrowSpawnPoint_Middle; //ȭ���� ������ ��ġ
    public Transform arrowSpawnPoint_Top; //ȭ���� ������ ��ġ
    public Transform arrowSpawnPoint_Bottom; //ȭ���� ������ ��ġ

    //ȭ�� ���� �Լ�
    public void CreateMiddle()
    {
        if (arrowPrefab != null && arrowSpawnPoint_Middle != null)
        {
            //ȭ�� �������� ������ ��ġ�� ����
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint_Middle.position, arrowSpawnPoint_Middle.rotation);
            //ȭ���� ������ �� �߰����� ������ �ʿ��ϴٸ� ���⿡ �ۼ�
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
