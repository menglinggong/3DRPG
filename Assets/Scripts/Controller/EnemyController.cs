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

    #endregion

    #region ���˵Ļ�������

    /// <summary>
    /// ����״̬
    /// </summary>
    private EnemyStates enemy_State;

    /// <summary>
    /// ���˳�ʼ״̬�Ƿ�Ϊվ׮
    /// </summary>
    public bool IsGuard = true;

    /// <summary>
    /// ��Ѱ�뾶
    /// </summary>
    [Header("Basic Settings")]
    public float SightRadius;

    /// <summary>
    /// ����׷���ĳ�ʼ�ٶ�
    /// </summary>
    private float speed;

    /// <summary>
    /// ����Ŀ��
    /// </summary>
    private GameObject attackTarget;

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
        speed = agent.speed;
    }

    private void Update()
    {
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
        //Ѱ����ң���׷��

        bool isFoundPlayer = FoundPlayer();
        if (isFoundPlayer)
        {
            enemy_State = EnemyStates.CHASE;
        }

        switch (enemy_State)
        {
            case EnemyStates.GUARD:
                //��������ʱ�����˵��ٶ�Ϊ׷��ʱ��һ��
                agent.speed = speed / 2f;
                break;
            case EnemyStates.PATROL:
                //����Ѳ��ʱ�����˵��ٶ�Ϊ׷��ʱ��һ��
                agent.speed = speed / 2f;

                break;
            case EnemyStates.CHASE:
                //TODO:׷�����
                //TODO:������ѣ��ص���һ��״̬
                //TODO:�ڹ�����Χ���򹥻�
                //TODO:��϶���
                //����׷��ʱ�����˵��ٶ�����

                isWalk = false;
                isChase = true;

                agent.speed = speed;

                if (!isFoundPlayer)
                {
                    enemy_State = IsGuard ? EnemyStates.GUARD : EnemyStates.PATROL;
                    isFollow = false;
                    //��ʧĿ�꣬����ֹͣ�ƶ�
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
