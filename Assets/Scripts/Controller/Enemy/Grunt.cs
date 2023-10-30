using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 兽人控制器
/// </summary>
public class Grunt : EnemyController
{
    /// <summary>
    /// 推的力
    /// </summary>
    [Header("技能")]
    public float KickForce = 10;

    /// <summary>
    /// 推玩家
    /// </summary>
    public void KickOff()
    {
        if (TargetInAttackRange() && transform.IsFacingTarget(attackTarget.transform))
        {
            this.transform.LookAt(attackTarget.transform);

            Vector3 dir = (attackTarget.transform.position - this.transform.position).normalized;

            //击退玩家
            NavMeshAgent targetAgent = attackTarget.GetComponent<NavMeshAgent>();
            targetAgent.isStopped = true;
            targetAgent.velocity = dir * KickForce;

            //播放玩家眩晕动画
            if (!attackTarget.GetComponent<CharacterStats>().IsDefence)
                attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}
