using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���˿�����
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
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
    private CharacterStats characterStats;

    #endregion

    #region ���˵Ļ�������

    /// <summary>
    /// ����״̬
    /// </summary>
    private EnemyStates enemy_State;

    /// <summary>
    /// ����׷���ĳ�ʼ�ٶ�
    /// </summary>
    private float speed;

    /// <summary>
    /// ����Ŀ��
    /// </summary>
    private GameObject attackTarget;

    /// <summary>
    /// Ѳ�ߵ�
    /// </summary>
    private Vector3 wayPoint;

    /// <summary>
    /// ���˳�ʼλ��
    /// </summary>
    private Vector3 enemyStartPoint;

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
    /// ��Ѳ�ߵ���Ĵ�������ʱ��
    /// </summary>
    public float LookAroundTime;

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

    #endregion

    #region �����Ĳ���

    bool isWalk;
    bool isChase;
    bool isFollow;

    #endregion

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        characterStats = this.GetComponent<CharacterStats>();
        speed = agent.speed;
    }

    private void Start()
    {
        enemy_State = IsGuard ? EnemyStates.GUARD : EnemyStates.PATROL;
        //���õ��˳�ʼλ����Ѳ��λ��
        enemyStartPoint = this.transform.position;
        GetNewWayPoint();
    }

    private void Update()
    {
        //���������ʱ
        if (lastAttackTime > 0)
        {
            lastAttackTime -= Time.deltaTime;
        }

        SwitchEnemyState();
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
    /// �л����˵�״̬
    /// </summary>
    void SwitchEnemyState()
    {
        bool isFoundPlayer = FoundPlayer();

        switch (enemy_State)
        {
            case EnemyStates.GUARD:
                //��������ʱ�����˵��ٶ�Ϊ׷��ʱ��һ�룬ʹ�ó˷������ıȳ���С
                agent.speed = speed * 0.5f;
                break;
            case EnemyStates.PATROL:

                isChase = false;
                //����Ѳ��ʱ�����˵��ٶ�Ϊ׷��ʱ��һ��
                agent.speed = speed * 0.5f;

                if (isFoundPlayer)
                {
                    enemy_State = EnemyStates.CHASE;
                }
                else
                {
                    //��Ѳ�ߵ�
                    if (Vector3.Distance(transform.position, wayPoint) <= agent.stoppingDistance)
                    {
                        isWalk = false;

                        //����Ĵ�������ʱ�䵽��
                        if(remainLookAroundTime <= 0)
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
                        //�ƶ���Ѳ�ߵ�
                        isWalk = true;
                        agent.destination = wayPoint;
                    }
                }

                break;
            case EnemyStates.CHASE:

                //׷�����
                isWalk = false;
                isChase = true;
                //����׷��ʱ�����˵��ٶ�����
                agent.speed = speed;

                if (!isFoundPlayer)
                {
                    isFollow = false;

                    if(remainLookAroundTime > 0)
                    {
                        //��ʧĿ�꣬����ֹͣ�ƶ�������Ĵ������ļ�ʱδ�������ȴ��Ĵ�����
                        agent.destination = transform.position;
                        remainLookAroundTime -= Time.deltaTime;
                    }
                    else
                    {
                        //�����������ص��ϸ�״̬
                        enemy_State = IsGuard ? EnemyStates.GUARD : EnemyStates.PATROL;
                    }
                }
                else
                {
                    //�ڹ�����Χ���򹥻�
                    if(TargetInAttackRange())
                    {
                        isFollow = false;
                        agent.isStopped = true;

                        if(lastAttackTime <= 0)
                        {
                            //����
                            Attack();
                        }
                    }
                    else
                    {
                        //������ң�׷��
                        isFollow = true;
                        agent.isStopped = false;
                        agent.destination = attackTarget.transform.position;
                    }
                }

                break;
            case EnemyStates.DEAD:
                break;
        }
    }

    /// <summary>
    /// ����Ѱ�뾶���������
    /// </summary>
    /// <returns></returns>
    private bool FoundPlayer()
    {
        //ʹ��Physics.OverlapSphere������ԭ������Ե���λ��Ϊԭ�㣬����һ���뾶Ϊ��Ѱֵ�����壬�ж������ڵ�������ײ���Ƿ������
        //ע�⣺ʹ��layer����ʱ��д��Ϊ1<<LayerMask.NameToLayer("Player")����ʾֻ���㼶ΪPlayer����ײ��
        Collider[] colliders = Physics.OverlapSphere(transform.position, SightRadius, 1<<LayerMask.NameToLayer("Player"));
        //�Ƚ�Ŀ���ƿ�
        attackTarget = null;

        if (colliders == null || colliders.Length == 0)
            return false;
        //���ҵ���ң��������Ŀ�긳ֵ
        attackTarget = colliders[0].gameObject;
        return true;
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
        //�ж��Ƿ񱩻���Random.value��ֵ��0~1�����ֵ
        characterStats.IsCritical = Random.value <= characterStats.CriticalChance;
        
        transform.LookAt(attackTarget.transform);

        //���������򲥷ű������������򲥷���ͨ��������
        animator.SetBool("CriticalAttack", characterStats.IsCritical);
        animator.SetTrigger("Attack");

        //���ü�ʱ
        lastAttackTime = characterStats.CoolDown;
    }

    /// <summary>
    /// ����˺����ڲ��Ŷ���ʱ����
    /// </summary>
    public void Hit()
    {
        if(TargetInAttackRange())
            characterStats.TakeDamage(characterStats, attackTarget.GetComponent<CharacterStats>());
    }

    /// <summary>
    /// Ŀ���Ƿ��ڹ���������
    /// </summary>
    /// <returns></returns>
    private bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, this.transform.position) <= characterStats.AttackRange;

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
}

/// <summary>
/// ����״̬
/// </summary>
public enum EnemyStates
{
    //վ׮/�����������Լ�Ѳ�ߣ�
    GUARD,
    //Ѳ��
    PATROL,
    //׷��
    CHASE,
    //����
    DEAD
}
