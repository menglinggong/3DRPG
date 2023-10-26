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
    #region ���

    /// <summary>
    /// ����
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// ����������
    /// </summary>
    private Animator animator;

    /// <summary>
    /// ��ҵ���ֵ
    /// </summary>
    private CharacterStats characterStats;

    #endregion

    #region ��ҵĻ�������

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

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    private bool isDie = false;

    #endregion



    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        characterStats = this.GetComponent<CharacterStats>();
        characterStats.CurrentHealth = characterStats.MaxHealth;

        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void Start()
    {
        //�¼���
        MouseManager.Instance.OnMouseClicked += MoveToTargetPos;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }

    private void OnDestroy()
    {
        MouseManager.Instance.OnMouseClicked -= MoveToTargetPos;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }

    private void Update()
    {
        SwitchAnimation(); 

        //���㹥�����ʱ��
        if(lastAttackTime > 0)
            lastAttackTime -= Time.deltaTime;

        if(characterStats.CurrentHealth <= 0 && !isDie)
        {
            isDie = true;
            agent.enabled = false;
            animator.SetTrigger("Die");

            Destroy(this.gameObject, 2);
        }
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
        
        //����˾�����ڹ������룬�ƶ�
        while(Vector3.Distance(this.transform.position, enemyTarget.transform.position) > characterStats.AttackRange)
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
            Attack();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    private void Attack()
    {
        //�ж��Ƿ񱩻���Random.value��ֵ��0~1�����ֵ
        characterStats.IsCritical = UnityEngine.Random.value <= characterStats.CriticalChance;

        transform.LookAt(enemyTarget.transform);

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
        characterStats.TakeDamage(characterStats, enemyTarget.GetComponent<CharacterStats>());
    }
}
