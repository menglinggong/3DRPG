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

    /// <summary>
    /// 动画控制器
    /// </summary>
    private Animator animator;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        //事件绑定
        MouseManager.Instance.OnMouseClicked += MoveToTargetPos;
    }

    private void Update()
    {
        SwitchAnimation(); 
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
        
        agent.destination = targetpos;
    }
}
