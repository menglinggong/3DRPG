using RPG.Skill;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���˿�����
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    #region ���

    /// <summary>
    /// ����
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// ���˵Ķ���������
    /// </summary>
    private Animator animator;

    /// <summary>
    /// ���˵���ֵ
    /// </summary>
    protected CharacterStats characterStats;

    /// <summary>
    /// ���˵���ײ��
    /// </summary>
    private Collider collider;

    /// <summary>
    /// ���ܹ�����
    /// </summary>
    private CharacterSkillManager characterSkillManager;

    /// <summary>
    /// ����ϵͳ
    /// </summary>
    private CharacterSkillSystem characterSkillSystem;

    #endregion

    #region ���˵Ļ�������

    /// <summary>
    /// ����Ŀ��
    /// </summary>
    protected GameObject attackTarget;

    /// <summary>
    /// Ѳ�ߵ�
    /// </summary>
    private Vector3 wayPoint;

    /// <summary>
    /// ���˳�ʼλ��
    /// </summary>
    private Vector3 enemyStartPoint;

    /// <summary>
    /// ��ʼ����
    /// </summary>
    private Quaternion enemyStartQuaternion;

    /// <summary>
    /// ��Ѱ�뾶
    /// </summary>
    [Header("Basic Settings")]
    public float SightRadius;

    /// <summary>
    /// ���˳�ʼ״̬�Ƿ�Ϊվ׮
    /// </summary>
    public bool IsGuard = true;

    /// <summary>
    /// �Ĵ������ļ�ʱ��
    /// </summary>
    private float remainLookAroundTime;

    /// <summary>
    /// �����ļ�ʱ��
    /// </summary>
    private float lastAttackTime = 0;

    /// <summary>
    /// Ѳ�߷�Χ
    /// </summary>
    [Header("Patrol State")]
    public float PatrolRange;

    /// <summary>
    /// ��Ѳ�ߵ���Ĵ�������ʱ��
    /// </summary>
    public float LookAroundTime;

    /// <summary>
    /// ��ʼֹͣ����
    /// </summary>
    private float stopDistance;

    #endregion

    #region �����Ĳ���

    bool isWalk = false;
    bool isChase = false;
    bool isFollow = false;
    bool isDie = false;

    #endregion

    #region ����

    /// <summary>
    /// ����Ƿ�����
    /// </summary>
    private bool isPlayerDie = false;

    /// <summary>
    /// ����׷���ĳ�ʼ�ٶ�
    /// </summary>
    private float speed;

    public CharacterStats CharacterStats
    {
        get { return characterStats; }
    }

    public bool IsPlayerDie
    {
        get
        {
            return isPlayerDie;
        }
    }

    #endregion

    protected Coroutine turnRoundCoroutine;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        characterStats = this.GetComponent<CharacterStats>();
        characterSkillManager = this.GetComponent<CharacterSkillManager>();
        characterSkillSystem = this.GetComponent<CharacterSkillSystem>();
        
        collider = this.GetComponent<Collider>();
        speed = agent.speed;
        stopDistance = agent.stoppingDistance;

        //���õ��˳�ʼλ����Ѳ��λ��
        enemyStartPoint = this.transform.position;
        enemyStartQuaternion = this.transform.rotation;
    }

    private void Start()
    {
        agent.speed = characterStats.CharacterData.MoveSpeed;
        GetNewWayPoint();

        //agent.stoppingDistance = characterStats.CharacterData.AttackRange;
        GameManager.Instance.AddEndGameObserver(this);

        characterStats.CharacterData.CurrentHealth = characterStats.CharacterData.MaxHealth;
    }

    private void OnDisable()
    {
        GameManager.Instance.RemoveEndGameObserver(this);
    }

    private void Update()
    {
        //���������ʱ
        if (lastAttackTime > 0)
        {
            lastAttackTime -= Time.deltaTime;
        }

        //����������Ͳ�ȥ�л����˵�״̬��
        if (isPlayerDie)
            return;

        SwitchAnimation();
    }

    /// <summary>
    /// ���ö������������ƶ����л�
    /// </summary>
    private void SwitchAnimation()
    {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("Chase", isChase);
        animator.SetBool("Follow", isFollow);
    }

    /// <summary>
    /// ����Ѱ�뾶���������
    /// </summary>
    /// <returns></returns>
    public bool FoundPlayer()
    {
        //ʹ��Physics.OverlapSphere������ԭ������Ե���λ��Ϊԭ�㣬����һ���뾶Ϊ��Ѱֵ�����壬�ж������ڵ�������ײ���Ƿ������
        //ע�⣺ʹ��layer����ʱ��д��Ϊ1<<LayerMask.NameToLayer("Player")����ʾֻ���㼶ΪPlayer����ײ��
        //Collider[] colliders = Physics.OverlapSphere(transform.position, SightRadius, 1<<LayerMask.NameToLayer("Player"));
        Collider[] colliders = Physics.OverlapSphere(transform.position, SightRadius);
        //�Ƚ�Ŀ���ƿ�
        
        //if (colliders == null || colliders.Length == 0)
        //    return false;

        foreach (Collider collider in colliders)
        {
            if(collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;

        ////���ҵ���ң��������Ŀ�긳ֵ
        //attackTarget = colliders[0].gameObject;
        //return true;
    }

    /// <summary>
    /// ����õ���һ�����Ѳ�ߵ�
    /// </summary>
    private void GetNewWayPoint()
    {
        //���¼�ʱ
        remainLookAroundTime = LookAroundTime;

        //�õ������ƫ��ֵ
        float random_X = Random.Range(-PatrolRange, PatrolRange);
        float ramdom_Z = Random.Range(-PatrolRange, PatrolRange);
        //�õ����λ��
        Vector3 randomPos = new Vector3(enemyStartPoint.x + random_X, enemyStartPoint.y, enemyStartPoint.z + ramdom_Z);

        NavMeshHit hit;
        //�ж����λ���Ƿ���ƶ��������ƶ��Ļ�ʹ�õ��˵�ǰλ��
        wayPoint = NavMesh.SamplePosition(randomPos, out hit, PatrolRange, 1)? hit.position : transform.position;
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Attack()
    {
        //ת�����
        Turnround(attackTarget.transform.position);

        characterStats.CalculateCritical(characterStats);

        if (TargetInAttackRange())
        {
            //���������򲥷ű������������򲥷���ͨ��������
            animator.SetBool("CriticalAttack", characterStats.IsCritical);
            animator.SetTrigger("Attack");
        }

        //���ü�ʱ
        lastAttackTime = 1f / characterStats.CharacterData.AttackSpeed;
    }

    /// <summary>
    /// ����˺����ڲ��Ŷ���ʱ����
    /// </summary>
    public void Hit()
    {
        //����������ڹ�����Χ�ھͻ��ܵ���������Ҫ�޸ģ���Ϊ��ҿ����ڹ�����Χ�ڣ�����������
        if(TargetInAttackRange() && transform.IsFacingTarget(attackTarget.transform))
        {
            CharacterStats targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    /// <summary>
    /// Ŀ���Ƿ��ڹ���������
    /// </summary>
    /// <returns></returns>
    public bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, this.transform.position) <= characterStats.CharacterData.AttackRange;
        }

        return false;
    }

    /// <summary>
    /// Ŀ���Ƿ��ڼ��ܷ�Χ��
    /// </summary>
    /// <returns></returns>
    public bool TargetInSkillRange()
    {
        if (attackTarget == null || characterSkillManager == null || characterSkillManager.skillDatas == null)
            return false;

        foreach (var skill in characterSkillManager.skillDatas)
        {
            if (Vector3.Distance(attackTarget.transform.position, this.transform.position) <= skill.attackDistance)
                return true;
        }

        return false;
    }

    /// <summary>
    /// ѡ�иõ���ʱ����Gizmos
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        //������Ѱ�뾶
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, SightRadius);
    }

    /// <summary>
    /// ������Ϸ�Ĺ㲥
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void EndNotify()
    {
        isPlayerDie = true;
        //animator.SetBool("Win", true);
        //isWalk = false;
        //isChase = false;
        //isFollow = false;
        //attackTarget = null;
    }

    #region ��ͬ״̬��ִ�еķ���

    /// <summary>
    /// Ѳ�߷���
    /// </summary>
    public void Patrol()
    {
        isChase = false;
        isFollow = false;
        //����Ѳ��ʱ�����˵��ٶ�Ϊ׷��ʱ��һ��
        agent.speed = speed * 0.5f;
        agent.stoppingDistance = stopDistance;
        //��Ѳ�ߵ�
        if (Vector3.Distance(transform.position, wayPoint) <= agent.stoppingDistance)
        {
            isWalk = false;

            //����Ĵ�������ʱ�䵽��
            if (remainLookAroundTime <= 0)
            {
                //��ȡ��һ��Ѳ�ߵ�
                GetNewWayPoint();
            }
            else
            {
                //�����Ĵ�������ͬʱ��ʱ
                remainLookAroundTime -= Time.deltaTime;
            }
        }
        else
        {
            //����Ĵ�������ʱ�䵽��
            if (remainLookAroundTime <= 0)
            {
                isWalk = true;
                agent.isStopped = false;
                agent.destination = wayPoint;
            }
            else
            {
                isWalk = false;
                agent.isStopped = true;
                //�����Ĵ�������ͬʱ��ʱ
                remainLookAroundTime -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// ����Ѳ��״̬
    /// </summary>
    public void EnterPatrol()
    {
        remainLookAroundTime = LookAroundTime;
        wayPoint = enemyStartPoint;
        Patrol();
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void Guard()
    {
        isChase = false;
        isFollow = false;
        //��������ʱ�����˵��ٶ�Ϊ׷��ʱ��һ�룬ʹ�ó˷������ıȳ���С
        agent.speed = speed * 0.5f;

        agent.stoppingDistance = stopDistance;
        //�����ǰλ�ò��ǳ�ʼλ�ã���ص���ʼλ�ã��˷�����Vector3.Distance����һ������������С
        if (Vector3.Distance(transform.position, enemyStartPoint) > stopDistance)
        {
            //����Ĵ�������ʱ�䵽��
            if (remainLookAroundTime <= 0)
            {
                isWalk = true;
                agent.isStopped = false;
                agent.destination = enemyStartPoint;
            }
            else
            {
                isWalk = false;
                agent.isStopped = true;
                //�����Ĵ�������ͬʱ��ʱ
                remainLookAroundTime -= Time.deltaTime;
            }
        }
        else
        {
            isWalk = false;
            if (this.transform.rotation != enemyStartQuaternion)
            {
                Vector3 dir = enemyStartQuaternion * Vector3.forward;
                Turnround(dir + this.transform.position);
            }
        }
    }

    /// <summary>
    /// ��������״̬
    /// </summary>
    public void EnterGuard()
    {
        remainLookAroundTime = LookAroundTime;
        Guard();
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Dead()
    {
        agent.enabled = false;
        collider.enabled = false;

        isDie = true;
        //������������
        animator.SetTrigger("Die");
        //���ٸ�����
        Destroy(gameObject, 2f);
    }

    /// <summary>
    /// ׷��
    /// </summary>
    public void Chase()
    {
        //׷�����
        isWalk = false;
        isChase = true;
        //����׷��ʱ�����˵��ٶ�����
        agent.speed = speed;

        //������ң�׷��
        isFollow = true;
        agent.isStopped = false;
        agent.destination = attackTarget.transform.position;
        agent.stoppingDistance = characterStats.CharacterData.AttackRange;
    }

    /// <summary>
    /// ����
    /// </summary>
    public void AttackState()
    {
        isFollow = false;
        agent.isStopped = true;

        if (TargetInSkillRange())
        {
            characterSkillSystem.UseRandomSkill();
        }
        else if (lastAttackTime <= 0)
        {
            //����
            Attack();
        }
    }

    /// <summary>
    /// ʤ��
    /// </summary>
    public void Win()
    {
        animator.SetBool("Win", true);
        isWalk = false;
        isChase = false;
        isFollow = false;
        attackTarget = null;
    }

    /// <summary>
    /// ת��
    /// </summary>
    /// <param name="targetPos"></param>
    private void Turnround(Vector3 targetPos)
    {
        if (turnRoundCoroutine != null)
            StopCoroutine(turnRoundCoroutine);

        turnRoundCoroutine = StartCoroutine(transform.TurnRound(targetPos, characterStats.CharacterData.TurnRoundSpeed));
    }

    #endregion
}

