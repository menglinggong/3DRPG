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
    /// <summary>
    /// ����
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// ����״̬
    /// </summary>
    private EnemyStates enemy_State;

    /// <summary>
    /// ��Ѱ�뾶
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
    /// �л����˵�״̬
    /// </summary>
    void SwitchEnemyState()
    {
        //Ѱ����ң���׷��
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
    /// ����Ѱ�뾶���������
    /// </summary>
    /// <returns></returns>
    private bool FoundPlayer()
    {
        //ʹ��Physics.OverlapSphere������ԭ������Ե���λ��Ϊԭ�㣬����һ���뾶Ϊ��Ѱֵ�����壬�ж������ڵ�������ײ���Ƿ������
        //ע�⣺ʹ��layer����ʱ��д��Ϊ1<<LayerMask.NameToLayer("Player")����ʾֻ���㼶ΪPlayer����ײ��
        Collider[] colliders = Physics.OverlapSphere(transform.position, SightRadius, 1<<LayerMask.NameToLayer("Player"));

        if (colliders == null || colliders.Length == 0)
            return false;

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
