using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 导航
    /// </summary>
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //事件绑定
        MouseManager.Instance.OnMouseClicked += MoveToTargetPos;
    }


    /// <summary>
    /// 移动到目标点
    /// </summary>
    /// <param name="targetpos"></param>
    private void MoveToTargetPos(Vector3 targetpos)
    {
        agent.destination = targetpos;
    }
}
