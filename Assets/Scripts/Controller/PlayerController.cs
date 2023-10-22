using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// ����������
    /// </summary>
    private Animator animator;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        //�¼���
        MouseManager.Instance.OnMouseClicked += MoveToTargetPos;
    }

    private void Update()
    {
        SwitchAnimation(); 
    }

    /// <summary>
    /// ������Ҷ����л�
    /// </summary>
    private void SwitchAnimation()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    /// <summary>
    /// �ƶ���Ŀ���
    /// </summary>
    /// <param name="targetpos"></param>
    private void MoveToTargetPos(Vector3 targetpos)
    {
        
        agent.destination = targetpos;
    }
}
