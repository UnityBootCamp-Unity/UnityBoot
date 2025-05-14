using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Action onDead;

    public void Dead()
    {
        onDead?.Invoke();
        gameObject.SetActive(false);
    }
}
