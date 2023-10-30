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

    /// <summary>
    /// 盾牌，防御状态下显示
    /// </summary>
    private GameObject defenceShield;

    #endregion

    #region 玩家的基本参数

    /// <summary>
    /// 敌人
    /// </summary>
    private GameObject attackTarget;

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

    /// <summary>
    /// 玩家初始停止距离
    /// </summary>
    private float stopDistance;

    #endregion



    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        characterStats = this.GetComponent<CharacterStats>();

        defenceShield = this.transform.Find("Defence").gameObject;
        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void Start()
    {
        characterStats.CurrentHealth = characterStats.MaxHealth;
        stopDistance = agent.stoppingDistance;
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
        if (isDie) return;

        SwitchAnimation(); 

        //计算攻击间隔时间
        if(lastAttackTime > 0)
            lastAttackTime -= Time.deltaTime;

        if(characterStats.CurrentHealth <= 0 && !isDie)
        {
            isDie = true;
            agent.enabled = false;
            animator.SetTrigger("Die");
            GameManager.Instance.NotifyObserver();
            //Destroy(this.gameObject, 2);

            MouseManager.Instance.OnMouseClicked -= MoveToTargetPos;
            MouseManager.Instance.OnEnemyClicked -= EventAttack;
        }

        Defence();
    }

    /// <summary>
    /// 玩家是否防御
    /// </summary>
    private void Defence()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Dizzy") || animator.GetCurrentAnimatorStateInfo(1).IsName("GetHit"))
            return;

        if (Input.GetMouseButton(1))
        {
            characterStats.IsDefence = true;
            agent.isStopped = true;
            defenceShield.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            characterStats.IsDefence = false;
            defenceShield.SetActive(false);
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
        attackTarget = null;

        agent.stoppingDistance = stopDistance;
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
            this.attackTarget = target;

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
        //设置玩家移动到攻击距离
        agent.stoppingDistance = characterStats.AttackRange;
        //离敌人距离大于攻击距离，移动
        while (Vector3.Distance(this.transform.position, attackTarget.transform.position) > characterStats.AttackRange)
        {
            agent.destination = attackTarget.transform.position;
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

        transform.LookAt(attackTarget.transform);

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
        if (attackTarget == null) return;

        if (attackTarget.CompareTag("AttackAble"))
        {
            Rock rock = attackTarget.GetComponent<Rock>();
            if(rock != null)
            {
                rock.GetComponent<Rigidbody>().velocity = Vector3.one;
                rock.rockState = RockStates.HitEnemy;
                rock.RockDamage = characterStats.GetRealDamage();

                //向石头施加一个往玩家前上方的力
                rock.GetComponent<Rigidbody>().AddForce((this.transform.forward + Vector3.up * 0.5f).normalized * rock.HitPlayerForce, ForceMode.Impulse);
            }
        }
        else
        {
            CharacterStats targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }

        
    }
}
