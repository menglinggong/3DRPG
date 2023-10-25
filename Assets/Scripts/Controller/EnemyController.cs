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
    public EnemyStates Enemy_State;

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
        switch(Enemy_State)
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
