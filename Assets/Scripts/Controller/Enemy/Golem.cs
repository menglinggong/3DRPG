using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 石头人控制器
/// </summary>
public class Golem : EnemyController
{
    /// <summary>
    /// 推的力
    /// </summary>
    public float KickForce = 30;

    /// <summary>
    /// 击退玩家
    /// </summary>
    public void KickOffAndHit()
    {
        if(TargetInAttackRange() && transform.IsFacingTarget(attackTarget.transform))
        {
            Vector3 dir = (attackTarget.transform.position - transform.position).normalized;

            //击退玩家
            NavMeshAgent targetAgent = attackTarget.GetComponent<NavMeshAgent>();
            targetAgent.isStopped = true;
            targetAgent.velocity = dir * KickForce;

            //播放玩家眩晕动画
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            //造成伤害
            characterStats.TakeDamage(characterStats, attackTarget.GetComponent<CharacterStats>());
        }
    }
}
