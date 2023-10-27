using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ʯͷ�˿�����
/// </summary>
public class Golem : EnemyController
{
    /// <summary>
    /// �Ƶ���
    /// </summary>
    public float KickForce = 30;

    /// <summary>
    /// �������
    /// </summary>
    public void KickOffAndHit()
    {
        if(TargetInAttackRange() && transform.IsFacingTarget(attackTarget.transform))
        {
            Vector3 dir = (attackTarget.transform.position - transform.position).normalized;

            //�������
            NavMeshAgent targetAgent = attackTarget.GetComponent<NavMeshAgent>();
            targetAgent.isStopped = true;
            targetAgent.velocity = dir * KickForce;

            //�������ѣ�ζ���
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            //����˺�
            characterStats.TakeDamage(characterStats, attackTarget.GetComponent<CharacterStats>());
        }
    }
}
