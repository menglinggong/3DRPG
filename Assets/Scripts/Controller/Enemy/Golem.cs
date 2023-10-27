using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    /// 石头人手的部位
    /// </summary>
    public Transform Hand;

    /// <summary>
    /// 石头示例
    /// </summary>
    public GameObject RockPrefab;

    /// <summary>
    /// 创建出的石头
    /// </summary>
    private GameObject rock;

    /// <summary>
    /// Animation event
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

    /// <summary>
    /// Animation event
    /// 创建石头
    /// </summary>
    public void CreateRock()
    {
        rock = ObjectPool.Instance.GetObject(RockPrefab.name);

        if(rock == null )
        {
            rock = Instantiate(RockPrefab, Hand.position, Quaternion.identity);
            rock.name = RockPrefab.name;
        }

        rock.SetActive(true);
        rock.GetComponent<Rigidbody>().isKinematic = true;
        rock.transform.SetParent(Hand, false);
        rock.transform.localPosition = Vector3.zero;
        rock.transform.localRotation = Quaternion.identity;
        rock.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 计算石头的伤害
    /// </summary>
    private void CalculateRockDamage()
    {
        characterStats.IsCritical = Random.value <= characterStats.CriticalChance;

        rock.GetComponent<Rock>().RockDamage = characterStats.GetRealDamage();
    }

    /// <summary>
    /// Animation event
    /// 投掷石头
    /// </summary>
    public void HurlRock()
    {
        Rock go = rock.GetComponent<Rock>();
        rock.transform.SetParent(null);
        go.Target = this.attackTarget;
        rock.GetComponent<Rigidbody>().isKinematic = false;

        CalculateRockDamage();
        go.FlyToTarget();
    }

}
