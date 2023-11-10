using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���˿�����
/// </summary>
public class Grunt : EnemyController
{
    /// <summary>
    /// �Ƶ���
    /// </summary>
    [Header("����")]
    public float KickForce = 10;

    /// <summary>
    /// �����
    /// </summary>
    public void KickOff()
    {
        if (TargetInAttackRange() && transform.IsFacingTarget(attackTarget.transform))
        {
            //ת�����
            if (turnRoundCoroutine != null)
                StopCoroutine(turnRoundCoroutine);

            turnRoundCoroutine = StartCoroutine(transform.TurnRound(attackTarget.transform.position, characterStats.CharacterData.TurnRoundSpeed));

            Vector3 dir = (attackTarget.transform.position - this.transform.position).normalized;

            //�������
            NavMeshAgent targetAgent = attackTarget.GetComponent<NavMeshAgent>();
            targetAgent.isStopped = true;
            targetAgent.velocity = dir * KickForce;

            //�������ѣ�ζ���
            if (!attackTarget.GetComponent<CharacterStats>().IsDefence)
                attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}
