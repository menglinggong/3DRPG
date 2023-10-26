using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region 组件

    /// <summary>
    /// 导航
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// 动画控制器
    /// </summary>
    private Animator animator;

    /// <summary>
    /// 玩家的数值
    /// </summary>
    private CharacterStats characterStats;

    #endregion

    #region 玩家的基本参数

    /// <summary>
    /// 敌人
    /// </summary>
    private GameObject enemyTarget;

    /// <summary>
    /// 攻击的协程
    /// </summary>
    private Coroutine AttackCoroutine;

    /// <summary>
    /// 上一次攻击的时间
    /// </summary>
    private float lastAttackTime;

    /// <summary>
    /// 是否死亡
    /// </summary>
    private bool isDie = false;

    #endregion



    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        characterStats = this.GetComponent<CharacterStats>();
        characterStats.CurrentHealth = characterStats.MaxHealth;

        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void Start()
    {
        //事件绑定
        MouseManager.Instance.OnMouseClicked += MoveToTargetPos;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }

    private void OnDestroy()
    {
        MouseManager.Instance.OnMouseClicked -= MoveToTargetPos;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }

    private void Update()
    {
        SwitchAnimation(); 

        //计算攻击间隔时间
        if(lastAttackTime > 0)
            lastAttackTime -= Time.deltaTime;

        if(characterStats.CurrentHealth <= 0 && !isDie)
        {
            isDie = true;
            agent.enabled = false;
            animator.SetTrigger("Die");

            Destroy(this.gameObject, 2);
        }
    }

    /// <summary>
    /// 控制玩家动画切换
    /// </summary>
    private void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    /// <summary>
    /// 移动到目标点
    /// </summary>
    /// <param name="targetpos"></param>
    private void MoveToTargetPos(Vector3 targetpos)
    {
        //使玩家可移动
        agent.isStopped = false;
        //停止移动到敌人的协程
        if (AttackCoroutine != null)
            StopCoroutine(AttackCoroutine);
        enemyTarget = null;

        agent.destination = targetpos;
    }

    /// <summary>
    /// 攻击目标
    /// </summary>
    /// <param name="target"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void EventAttack(GameObject target)
    {
        if(target != null)
        {
            this.enemyTarget = target;

            //停止移动到敌人的协程
            if (AttackCoroutine != null)
                StopCoroutine(AttackCoroutine);

            AttackCoroutine = StartCoroutine(MoveToAttackTarget());
        }
    }

    /// <summary>
    /// 移动到目标
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToAttackTarget()
    {
        //使玩家可移动
        agent.isStopped = false;
        
        //离敌人距离大于攻击距离，移动
        while(Vector3.Distance(this.transform.position, enemyTarget.transform.position) > characterStats.AttackRange)
        {
            agent.destination = enemyTarget.transform.position;
            yield return null;
        }

        //进入攻击范围，进行攻击
        //1.使玩家不可移动
        agent.isStopped = true;
        //2.计算攻击间隔时间
        if(lastAttackTime <= 0)
        {
            Attack();
        }
    }

    /// <summary>
    /// 攻击
    /// </summary>
    private void Attack()
    {
        //判断是否暴击，Random.value的值是0~1的随机值
        characterStats.IsCritical = UnityEngine.Random.value <= characterStats.CriticalChance;

        transform.LookAt(enemyTarget.transform);

        //若暴击了则播放暴击动画，否则播放普通攻击动画
        animator.SetBool("CriticalAttack", characterStats.IsCritical);
        animator.SetTrigger("Attack");

        //重置计时
        lastAttackTime = characterStats.CoolDown;
    }

    /// <summary>
    /// 造成伤害，在播放动画时调用
    /// </summary>
    public void Hit()
    {
        characterStats.TakeDamage(characterStats, enemyTarget.GetComponent<CharacterStats>());
    }
}
