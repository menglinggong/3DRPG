using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敌人控制器
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// 导航
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// 敌人状态
    /// </summary>
    private EnemyStates enemy_State;

    /// <summary>
    /// 搜寻半径
    /// </summary>
    [Header("Basic Settings")]
    public float SightRadius;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        SwitchEnemyState();
    }

    /// <summary>
    /// 切换敌人的状态
    /// </summary>
    void SwitchEnemyState()
    {
        //寻找玩家，并追击
        if(FoundPlayer())
        {
            enemy_State = EnemyStates.CHASE;
            Debug.Log("found player!!");
        }

        switch (enemy_State)
        {
            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATROL:
                break;
            case EnemyStates.CHASE:
                break;
            case EnemyStates.DEAD:
                break;
        }
    }

    /// <summary>
    /// 在搜寻半径内搜索玩家
    /// </summary>
    /// <returns></returns>
    private bool FoundPlayer()
    {
        //使用Physics.OverlapSphere方法，原理就是以敌人位置为原点，构建一个半径为搜寻值的球体，判断球体内的所有碰撞体是否有玩家
        //注意：使用layer限制时，写法为1<<LayerMask.NameToLayer("Player")，表示只检测层级为Player的碰撞体
        Collider[] colliders = Physics.OverlapSphere(transform.position, SightRadius, 1<<LayerMask.NameToLayer("Player"));

        if (colliders == null || colliders.Length == 0)
            return false;

        return true;
    }
}

/// <summary>
/// 敌人状态
/// </summary>
public enum EnemyStates
{
    //站桩/守卫（不会自己巡逻）
    GUARD,
    //巡逻
    PATROL,
    //追击
    CHASE,
    //死亡
    DEAD
}
