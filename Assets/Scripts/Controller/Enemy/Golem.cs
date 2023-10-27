using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ʯͷ�˿�����
/// </summary>
public class Golem : EnemyController
{
    /// <summary>
    /// �Ƶ���
    /// </summary>
    public float KickForce = 30;

    /// <summary>
    /// ʯͷ���ֵĲ�λ
    /// </summary>
    public Transform Hand;

    /// <summary>
    /// ʯͷʾ��
    /// </summary>
    public GameObject RockPrefab;

    /// <summary>
    /// ��������ʯͷ
    /// </summary>
    private GameObject rock;

    /// <summary>
    /// Animation event
    /// �������
    /// </summary>
    public void KickOffAndHit()
    {
        if(TargetInAttackRange() && transform.IsFacingTarget(attackTarget.transform))
        {
            Vector3 dir = (attackTarget.transform.position - transform.position).normalized;

            //�������
            NavMeshAgent targetAgent = attackTarget.GetComponent<NavMeshAgent>();
            targetAgent.isStopped = true;
            targetAgent.velocity = dir * KickForce;

            //�������ѣ�ζ���
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            //����˺�
            characterStats.TakeDamage(characterStats, attackTarget.GetComponent<CharacterStats>());
        }
    }

    /// <summary>
    /// Animation event
    /// ����ʯͷ
    /// </summary>
    public void CreateRock()
    {
        rock = ObjectPool.Instance.GetObject(RockPrefab.name);

        if(rock == null )
        {
            rock = Instantiate(RockPrefab, Hand.position, Quaternion.identity);
            rock.name = RockPrefab.name;
        }

        rock.SetActive(true);
        rock.GetComponent<Rigidbody>().isKinematic = true;
        rock.transform.SetParent(Hand, false);
        rock.transform.localPosition = Vector3.zero;
        rock.transform.localRotation = Quaternion.identity;
        rock.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Animation event
    /// Ͷ��ʯͷ
    /// </summary>
    public void HurlRock()
    {
        Rock go = rock.GetComponent<Rock>();
        rock.transform.SetParent(null);
        go.Target = this.attackTarget;
        rock.GetComponent<Rigidbody>().isKinematic = false;
        go.FlyToTarget();
    }

}
