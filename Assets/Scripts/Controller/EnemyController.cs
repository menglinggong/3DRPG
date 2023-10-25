using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// µÐÈË¿ØÖÆÆ÷
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// µ¼º½
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// µÐÈË×´Ì¬
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
    /// ÇÐ»»µÐÈËµÄ×´Ì¬
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
/// µÐÈË×´Ì¬
/// </summary>
public enum EnemyStates
{
    //Õ¾×®/ÊØÎÀ£¨²»»á×Ô¼ºÑ²Âß£©
    GUARD,
    //Ñ²Âß
    PATROL,
    //×·»÷
    CHASE,
    //ËÀÍö
    DEAD
}
