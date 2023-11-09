using RPG.Skill;
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

    /// <summary>
    /// 玩家的技能系统
    /// </summary>
    private CharacterSkillSystem characterSkillSystem;

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
        characterSkillSystem = this.GetComponent<CharacterSkillSystem>();

        defenceShield = this.transform.Find("Defence").gameObject;
        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void Start()
    {
        stopDistance = agent.stoppingDistance;
        //事件绑定
        MouseManager.Instance.OnMouseClicked += MoveToTargetPos;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }

    private void OnEnable()
    {
        SaveDataManager.Instance.LoadPlayerData();
        //玩家加载数据后发送消息，设置血条和经验条
        EventManager.Instance.Invoke(MessageConst.UpdateHealth, characterStats);
        EventManager.Instance.Invoke(MessageConst.UpdateExp, characterStats);
    }

    private void OnDisable()
    {
        SaveDataManager.Instance.SavePlayerData();
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

        //测试按Q放技能
        if (Input.GetKeyDown(KeyCode.Q))
        {
            characterSkillSystem.AttackUseSkill(1, false);
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
        if (lastAttackTime <= 0)
        {
            Attack();
        }
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    private void Attack()
    {
        //TODO:解决使用技能系统进行普通攻击时，无法计算攻击冷却的问题
        //使用技能系统进行普通攻击
        lastAttackTime = characterStats.AttackData.CoolDown;
        characterSkillSystem.AttackUseSkill(0, false);
    }

    /// <summary>
    /// 造成伤害，在播放动画时调用
    /// </summary>
    public void Hit()
    {
        //TODO:动画事件使用技能系统的方法
        if (attackTarget == null) return;

        if (attackTarget.CompareTag("AttackAble"))
        {
            Rock rock = attackTarget.GetComponent<Rock>();
            if (rock != null)
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
