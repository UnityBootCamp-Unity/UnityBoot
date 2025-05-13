using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject blockPrefab;
    public int width = 10;
    public int height = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                var go  = Instantiate(blockPrefab, new Vector3(i,j,0), Quaternion.identity);
                go.transform.parent = transform;
            }
        }
    }
}
