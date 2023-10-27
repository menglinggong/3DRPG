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
        if(attackTarget != null)
        {
            this.transform.LookAt(attackTarget.transform);

            Vector3 dir = (attackTarget.transform.position - this.transform.position).normalized;

            //�������
            NavMeshAgent targetAgent = attackTarget.GetComponent<NavMeshAgent>();
            targetAgent.isStopped = true;
            targetAgent.velocity = dir * KickForce;

            //�������ѣ�ζ���
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}
