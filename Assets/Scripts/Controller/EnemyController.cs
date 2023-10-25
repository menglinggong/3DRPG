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
    #region 组件

    /// <summary>
    /// 导航
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// 敌人的动画控制器
    /// </summary>
    private Animator animator;

    #endregion

    #region 敌人的基本参数

    /// <summary>
    /// 敌人状态
    /// </summary>
    private EnemyStates enemy_State;

    /// <summary>
    /// 敌人初始状态是否为站桩
    /// </summary>
    public bool IsGuard = true;

    /// <summary>
    /// 搜寻半径
    /// </summary>
    [Header("Basic Settings")]
    public float SightRadius;

    /// <summary>
    /// 敌人追击的初始速度
    /// </summary>
    private float speed;

    /// <summary>
    /// 攻击目标
    /// </summary>
    private GameObject attackTarget;

    #endregion

    #region 动画的参数

    bool isWalk;
    bool isChase;
    bool isFollow;

    #endregion

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        speed = agent.speed;
    }

    private void Update()
    {
        SwitchEnemyState();
        SwitchAnimation();
    }

    /// <summary>
    /// 设置动画参数，控制动画切换
    /// </summary>
    private void SwitchAnimation()
    {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("Chase", isChase);
        animator.SetBool("Follow", isFollow);
    }

    /// <summary>
    /// 切换敌人的状态
    /// </summary>
    void SwitchEnemyState()
    {
        //寻找玩家，并追击

        bool isFoundPlayer = FoundPlayer();
        if (isFoundPlayer)
        {
            enemy_State = EnemyStates.CHASE;
        }

        switch (enemy_State)
        {
            case EnemyStates.GUARD:
                //设置守卫时，敌人的速度为追击时的一半
                agent.speed = speed / 2f;
                break;
            case EnemyStates.PATROL:
                //设置巡逻时，敌人的速度为追击时的一半
                agent.speed = speed / 2f;

                break;
            case EnemyStates.CHASE:
                //TODO:追击玩家
                //TODO:玩家逃脱，回到上一个状态
                //TODO:在攻击范围内则攻击
                //TODO:配合动画
                //设置追击时，敌人的速度正常

                isWalk = false;
                isChase = true;

                agent.speed = speed;

                if (!isFoundPlayer)
                {
                    enemy_State = IsGuard ? EnemyStates.GUARD : EnemyStates.PATROL;
                    isFollow = false;
                    //丢失目标，立刻停止移动
                    agent.destination = transform.position;
                }
                else
                {
                    isFollow = true;
                    //TODO
                    agent.destination = attackTarget.transform.position;
                }

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
        //先将目标制空
        attackTarget = null;

        if (colliders == null || colliders.Length == 0)
            return false;
        //若找到玩家，则给攻击目标赋值
        attackTarget = colliders[0].gameObject;
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
