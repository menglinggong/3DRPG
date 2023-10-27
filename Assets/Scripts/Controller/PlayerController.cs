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
    private GameObject attackTarget;

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

    /// <summary>
    /// ��ҳ�ʼֹͣ����
    /// </summary>
    private float stopDistance;

    #endregion



    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        characterStats = this.GetComponent<CharacterStats>();
        
        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void Start()
    {
        characterStats.CurrentHealth = characterStats.MaxHealth;
        stopDistance = agent.stoppingDistance;
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
        if (isDie) return;

        SwitchAnimation(); 

        //���㹥�����ʱ��
        if(lastAttackTime > 0)
            lastAttackTime -= Time.deltaTime;

        if(characterStats.CurrentHealth <= 0 && !isDie)
        {
            isDie = true;
            agent.enabled = false;
            animator.SetTrigger("Die");
            GameManager.Instance.NotifyObserver();
            //Destroy(this.gameObject, 2);

            MouseManager.Instance.OnMouseClicked -= MoveToTargetPos;
            MouseManager.Instance.OnEnemyClicked -= EventAttack;
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
        attackTarget = null;

        agent.stoppingDistance = stopDistance;
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
            this.attackTarget = target;

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
        //��������ƶ�����������
        agent.stoppingDistance = characterStats.AttackRange;
        //����˾�����ڹ������룬�ƶ�
        while (Vector3.Distance(this.transform.position, attackTarget.transform.position) > characterStats.AttackRange)
        {
            agent.destination = attackTarget.transform.position;
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
        if (attackTarget == null) return;

        if (attackTarget.CompareTag("AttackAble"))
        {
            Rock rock = attackTarget.GetComponent<Rock>();
            if(rock != null)
            {
                rock.rockState = RockStates.HitEnemy;
                rock.RockDamage = characterStats.GetRealDamage();

                //��ʯͷʩ��һ�������ǰ�Ϸ�����
                rock.GetComponent<Rigidbody>().AddForce((this.transform.forward + Vector3.up).normalized * rock.HitPlayerForce, ForceMode.Impulse);
            }
        }
        else
        {
            characterStats.TakeDamage(characterStats, attackTarget.GetComponent<CharacterStats>());
        }

        
    }
}
