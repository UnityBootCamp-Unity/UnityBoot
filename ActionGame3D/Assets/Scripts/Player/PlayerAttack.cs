using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("데미지 수치")]
    [Tooltip("일반 공격 데미지")] public int NormalDamage = 10;
    [Tooltip("스킬 공격 데미지")] public int SkillDamage = 20;
    [Tooltip("대시 공격 데미지")] public int DashDamage = 30;

    [Header("타겟")]
    public NormalTarget normalTarget;
    public SkillTarget skillTarget;

    public void NormalAttack()
    {
        var targetList = new List<Collider>(normalTarget.targetList);

        foreach (var target in targetList)
        {
            var enemy = target.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                StartCoroutine(enemy.StartDamage(NormalDamage, transform.position, 0.5f, 0.5f));
            }
        }
    }

    public void DashAttack()
    {
        var targetList = new List<Collider>(skillTarget.targetList);

        foreach (var target in targetList)
        {
            var enemy = target.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                StartCoroutine(enemy.StartDamage(DashDamage, transform.position, 1f, 2f));
            }
        }
    }

    public void SkillAttack()
    {
        var targetList = new List<Collider>(skillTarget.targetList);

        foreach (var target in targetList)
        {
            var enemy = target.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                StartCoroutine(enemy.StartDamage(SkillDamage, transform.position, 1f, 2f));
            }
        }
    }
}
