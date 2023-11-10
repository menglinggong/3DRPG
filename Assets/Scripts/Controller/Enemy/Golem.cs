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
            if(!attackTarget.GetComponent<CharacterStats>().IsDefence)
                attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            //����˺�
            CharacterStats targetStats = targetAgent.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    /// <summary>
    /// Animation event
    /// ����ʯͷ
    /// </summary>
    public void CreateRock()
    {
        rock = ObjectPool.Instance.GetObject(RockPrefab.name, RockPrefab);

        if(rock == null )
        {
            rock = Instantiate(RockPrefab, Hand.position, Quaternion.identity);
            rock.name = RockPrefab.name;
        }
        rock.transform.localScale = Vector3.one;
        rock.SetActive(true);
        rock.GetComponent<Rigidbody>().isKinematic = true;
        rock.transform.SetParent(Hand, true);
        rock.transform.localPosition = Vector3.zero;
        rock.transform.localRotation = Quaternion.identity;
        //
    }

    /// <summary>
    /// ����ʯͷ���˺�
    /// </summary>
    private void CalculateRockDamage()
    {
        characterStats.IsCritical = Random.value <= characterStats.AttackData.CriticalChance;

        rock.GetComponent<Rock>().RockDamage = characterStats.GetRealDamage();
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
        rock.GetComponent<Rigidbody>().velocity = Vector3.one;
        rock.GetComponent<Rigidbody>().isKinematic = false;

        CalculateRockDamage();
        go.FlyToTarget();
    }

}
