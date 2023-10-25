using System;
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

    /// <summary>
    /// ����
    /// </summary>
    private GameObject enemyTarget;

    /// <summary>
    /// ������Э��
    /// </summary>
    private Coroutine AttackCoroutine;

    /// <summary>
    /// ��һ�ι�����ʱ��
    /// </summary>
    private float lastAttackTime;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        //�¼���
        MouseManager.Instance.OnMouseClicked += MoveToTargetPos;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }

    private void Update()
    {
        SwitchAnimation(); 

        //���㹥�����ʱ��
        if(lastAttackTime > 0)
            lastAttackTime -= Time.deltaTime;
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
        //ʹ��ҿ��ƶ�
        agent.isStopped = false;
        //ֹͣ�ƶ������˵�Э��
        if (AttackCoroutine != null)
            StopCoroutine(AttackCoroutine);
        enemyTarget = null;

        agent.destination = targetpos;
    }

    /// <summary>
    /// ����Ŀ��
    /// </summary>
    /// <param name="target"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void EventAttack(GameObject target)
    {
        if(target != null)
        {
            this.enemyTarget = target;

            //ֹͣ�ƶ������˵�Э��
            if (AttackCoroutine != null)
                StopCoroutine(AttackCoroutine);

            AttackCoroutine = StartCoroutine(MoveToAttackTarget());
        }
    }

    /// <summary>
    /// �ƶ���Ŀ��
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToAttackTarget()
    {
        //ʹ��ҿ��ƶ�
        agent.isStopped = false;
        //���ת����Ե���
        transform.LookAt(enemyTarget.transform);

        //����˾�����ڹ������룬�ƶ������������ݶ�Ϊ1��
        while(Vector3.Distance(this.transform.position, enemyTarget.transform.position) > 1)
        {
            agent.destination = enemyTarget.transform.position;
            yield return null;
        }

        //���빥����Χ�����й���
        //1.ʹ��Ҳ����ƶ�
        agent.isStopped = true;
        //2.���㹥�����ʱ��
        if(lastAttackTime <= 0)
        {
            //3.���Ź�������
            animator.SetTrigger("Attack");
            //4.������ȴʱ�䣨Ŀǰ��ȴʱ��̶�Ϊ0.5s��
            lastAttackTime = 0.5f;
        }
    }
}
