using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敌人控制器
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
public class EnemyController : MonoBehaviour, IEndGameObserver
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

    /// <summary>
    /// 敌人的数值
    /// </summary>
    protected CharacterStats characterStats;

    /// <summary>
    /// 敌人的碰撞器
    /// </summary>
    private Collider collider;

    #endregion

    #region 敌人的基本参数

    /// <summary>
    /// 敌人状态
    /// </summary>
    private EnemyStates enemy_State;

    /// <summary>
    /// 敌人追击的初始速度
    /// </summary>
    private float speed;

    /// <summary>
    /// 攻击目标
    /// </summary>
    protected GameObject attackTarget;

    /// <summary>
    /// 巡逻点
    /// </summary>
    private Vector3 wayPoint;

    /// <summary>
    /// 敌人初始位置
    /// </summary>
    private Vector3 enemyStartPoint;

    /// <summary>
    /// 初始朝向
    /// </summary>
    private Quaternion enemyStartQuaternion;

    /// <summary>
    /// 搜寻半径
    /// </summary>
    [Header("Basic Settings")]
    public float SightRadius;

    /// <summary>
    /// 敌人初始状态是否为站桩
    /// </summary>
    public bool IsGuard = true;

    /// <summary>
    /// 四处张望的计时器
    /// </summary>
    private float remainLookAroundTime;

    /// <summary>
    /// 攻击的计时器
    /// </summary>
    private float lastAttackTime = 0;

    /// <summary>
    /// 巡逻范围
    /// </summary>
    [Header("Patrol State")]
    public float PatrolRange;

    /// <summary>
    /// 到巡逻点后四处张望的时间
    /// </summary>
    public float LookAroundTime;

    #endregion

    #region 动画的参数

    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDie = false;

    #endregion

    /// <summary>
    /// 玩家是否死亡
    /// </summary>
    private bool isPlayerDie = false;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        characterStats = this.GetComponent<CharacterStats>();
        
        collider = this.GetComponent<Collider>();
        speed = agent.speed;
    }

    private void Start()
    {
        enemy_State = IsGuard ? EnemyStates.GUARD : EnemyStates.PATROL;
        //设置敌人初始位置与巡逻位置
        enemyStartPoint = this.transform.position;
        enemyStartQuaternion = this.transform.rotation;
        GetNewWayPoint();

        GameManager.Instance.AddEndGameObserver(this);

        characterStats.CurrentHealth = characterStats.MaxHealth;
    }

    //private void OnEnable()
    //{
    //    GameManager.Instance.AddEndGameObserver(this);
    //}

    private void OnDisable()
    {
        GameManager.Instance.RemoveEndGameObserver(this);
    }

    private void Update()
    {
        //攻击间隔计时
        if (lastAttackTime > 0)
        {
            lastAttackTime -= Time.deltaTime;
        }

        //玩家死亡，就不去切换敌人的状态了
        if (isPlayerDie)
            return;

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
        if (characterStats.CurrentHealth <= 0)
        {
            enemy_State = EnemyStates.DEAD;
        }

        bool isFoundPlayer = FoundPlayer();

        switch (enemy_State)
        {
            case EnemyStates.GUARD:

                isChase = false;
                //设置守卫时，敌人的速度为追击时的一半，使用乘法的消耗比除法小
                agent.speed = speed * 0.5f;

                if (isFoundPlayer)
                {
                    enemy_State = EnemyStates.CHASE;
                }
                else
                {
                    //如果当前位置不是初始位置，则回到初始位置，此方法与Vector3.Distance作用一样，但开销更小
                    if (Vector3.SqrMagnitude(transform.position - enemyStartPoint) > agent.stoppingDistance)
                    {
                        isWalk = true;
                        agent.isStopped = false;
                        agent.destination = enemyStartPoint;
                    }
                    else
                    {
                        isWalk = false;
                        
                        if(this.transform.rotation != enemyStartQuaternion)
                        {
                            //回到初始朝向
                            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, enemyStartQuaternion, 0.01f);
                        }
                    }
                }

                break;
            case EnemyStates.PATROL:

                isChase = false;
                //设置巡逻时，敌人的速度为追击时的一半
                agent.speed = speed * 0.5f;

                if (isFoundPlayer)
                {
                    enemy_State = EnemyStates.CHASE;
                }
                else
                {
                    //到巡逻点
                    if (Vector3.Distance(transform.position, wayPoint) <= agent.stoppingDistance)
                    {
                        isWalk = false;

                        //如果四处张望的时间到了
                        if(remainLookAroundTime <= 0)
                        {
                            //获取下一个巡逻点
                            GetNewWayPoint();
                            
                        }
                        else
                        {
                            //敌人四处看看，同时计时
                            remainLookAroundTime -= Time.deltaTime;
                        }
                    }
                    else
                    {
                        //移动到巡逻点
                        isWalk = true;
                        agent.isStopped = false;
                        agent.destination = wayPoint;
                    }
                }

                break;
            case EnemyStates.CHASE:

                //追击玩家
                isWalk = false;
                isChase = true;
                //设置追击时，敌人的速度正常
                agent.speed = speed;

                if (!isFoundPlayer)
                {
                    isFollow = false;

                    if(remainLookAroundTime > 0)
                    {
                        //丢失目标，立刻停止移动，如果四处张望的计时未结束，等待四处张望
                        agent.destination = transform.position;
                        remainLookAroundTime -= Time.deltaTime;
                    }
                    else
                    {
                        //张望结束，回到上个状态
                        enemy_State = IsGuard ? EnemyStates.GUARD : EnemyStates.PATROL;
                    }
                }
                else
                {
                    //在攻击范围内则攻击
                    if(TargetInAttackRange() || TargetInSkillRange())
                    {
                        isFollow = false;
                        agent.isStopped = true;

                        if(lastAttackTime <= 0)
                        {
                            //攻击
                            Attack();
                        }
                    }
                    else
                    {
                        //发现玩家，追击
                        isFollow = true;
                        agent.isStopped = false;
                        agent.destination = attackTarget.transform.position;
                    }
                }

                break;
            case EnemyStates.DEAD:

                agent.enabled = false;
                collider.enabled = false;

                isDie = true;
                //播放死亡动画
                animator.SetTrigger("Die");
                //销毁该物体
                Destroy(gameObject, 2f);
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

    /// <summary>
    /// 计算得到下一个随机巡逻点
    /// </summary>
    private void GetNewWayPoint()
    {
        //重新计时
        remainLookAroundTime = LookAroundTime;

        //得到随机的偏移值
        float random_X = Random.Range(-PatrolRange, PatrolRange);
        float ramdom_Z = Random.Range(-PatrolRange, PatrolRange);
        //得到随机位置
        Vector3 randomPos = new Vector3(enemyStartPoint.x + random_X, enemyStartPoint.y, enemyStartPoint.z + ramdom_Z);

        NavMeshHit hit;
        //判断随机位置是否可移动，不可移动的话使用敌人当前位置
        wayPoint = NavMesh.SamplePosition(randomPos, out hit, PatrolRange, 1)? hit.position : transform.position;
    }

    /// <summary>
    /// 攻击
    /// </summary>
    private void Attack()
    {
        //判断是否暴击，Random.value的值是0~1的随机值
        characterStats.IsCritical = Random.value <= characterStats.CriticalChance;
        //TODO:转向玩家因该有个过程，不能立刻转
        transform.LookAt(attackTarget.transform);

        if(TargetInAttackRange())
        {
            //若暴击了则播放暴击动画，否则播放普通攻击动画
            animator.SetBool("CriticalAttack", characterStats.IsCritical);
            animator.SetTrigger("Attack");
        }

        if(TargetInSkillRange())
        {
            animator.SetTrigger("Skill");
        }
       
        //重置计时
        lastAttackTime = characterStats.CoolDown;
    }

    /// <summary>
    /// 造成伤害，在播放动画时调用
    /// </summary>
    public void Hit()
    {
        //现在是玩家在攻击范围内就会受到攻击，需要修改，因为玩家可能在攻击范围内，但不在正面
        if(TargetInAttackRange() && transform.IsFacingTarget(attackTarget.transform))
            characterStats.TakeDamage(characterStats, attackTarget.GetComponent<CharacterStats>());
    }

    /// <summary>
    /// 目标是否在攻击距离内
    /// </summary>
    /// <returns></returns>
    public bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, this.transform.position) <= characterStats.AttackRange;

        return false;
    }

    /// <summary>
    /// 目标是否在技能范围内
    /// </summary>
    /// <returns></returns>
    public bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, this.transform.position) <= characterStats.SkillRange;

        return false;
    }

    /// <summary>
    /// 选中该敌人时绘制Gizmos
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        //绘制搜寻半径
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, SightRadius);
    }

    /// <summary>
    /// 结束游戏的广播
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void EndNotify()
    {
        isPlayerDie = true;
        animator.SetBool("Win", true);
        isWalk = false;
        isChase = false;
        isFollow = false;
        attackTarget = null;
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
