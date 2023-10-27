using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ʯͷ��״̬
/// </summary>
public enum RockStates
{
    HitPlayer,
    HitEnemy,
    HitNothing
}

/// <summary>
/// ʯͷ��Ͷ����ʯͷ
/// </summary>
public class Rock : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    private Rigidbody rigidbody;

    /// <summary>
    /// ʯͷ�ɳ�ʱ��ʩ�ӵ���
    /// </summary>
    public float Force;

    /// <summary>
    /// ײ�����ʱ����
    /// </summary>
    public float HitPlayerForce;

    /// <summary>
    /// Ŀ��
    /// </summary>
    [HideInInspector]
    public GameObject Target;

    /// <summary>
    /// ʯͷ��״̬
    /// </summary>
    [HideInInspector]
    public RockStates rockState = RockStates.HitPlayer;

    /// <summary>
    /// ����
    /// </summary>
    private Vector3 direction;

    /// <summary>
    /// ʯͷ���˺�ֵ���������ʯͷ�˴��������ҷ������
    /// </summary>
    [HideInInspector]
    public float RockDamage;

    private void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rockState = RockStates.HitPlayer;
    }

    private void FixedUpdate()
    {
        //TODO:���ʯͷ��ʯͷ������ʱ���ٶ�Ϊ0
        if(rigidbody.velocity.sqrMagnitude < 0.1f)
        {
            rockState = RockStates.HitNothing;
        }
    }

    /// <summary>
    /// ����Ŀ��
    /// </summary>
    public void FlyToTarget()
    {
        direction = (Target.transform.position - transform.position + Vector3.up).normalized;

        //ʩ���������Ʊ�ը��
        rigidbody.AddForce(direction *  Force, ForceMode.Impulse);
    }

    /// <summary>
    /// ��ײ
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {

        //TODO:�޸�Ϊ��Ұ��°�ť�������ģʽ���밴�°�ť���з���
        switch (rockState)
        {
            case RockStates.HitPlayer:
                //���˺���ҵ�״̬�£������������ң����������˺�
                if (collision.gameObject.CompareTag("Player"))
                {
                    NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
                    
                    agent.velocity = direction * HitPlayerForce;
                    agent.isStopped = true;

                    collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");

                    CharacterStats targetStats = collision.gameObject.GetComponent<CharacterStats>();
                    targetStats.TakeDamage(this.RockDamage, targetStats);
                }

                break;
            case RockStates.HitEnemy:
                //����ʯͷ��
                if(collision.gameObject.GetComponent<Golem>())
                {
                    NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();

                    agent.velocity = direction * HitPlayerForce;
                    agent.isStopped = true;

                    CharacterStats targetStats = collision.gameObject.GetComponent<CharacterStats>();
                    this.RockDamage = Target.GetComponent<CharacterStats>().GetRealDamage();

                    targetStats.TakeDamage(this.RockDamage, targetStats);
                }

                break;
            case RockStates.HitNothing:
                //StopAllCoroutines();
                //StartCoroutine(ReleaseRock());
                break;
        }
    }

    /// <summary>
    /// �Ž������
    /// </summary>
    /// <returns></returns>
    IEnumerator ReleaseRock()
    {
        yield return new WaitForSeconds(1f);

        ObjectPool.Instance.ReleaseObject(this.name, this.gameObject);
    }
}
