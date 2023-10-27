using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 石头的状态
/// </summary>
public enum RockStates
{
    HitPlayer,
    HitEnemy,
    HitNothing
}

/// <summary>
/// 石头人投掷的石头
/// </summary>
public class Rock : MonoBehaviour
{
    /// <summary>
    /// 刚体
    /// </summary>
    private Rigidbody rigidbody;

    /// <summary>
    /// 石头飞出时被施加的力
    /// </summary>
    public float Force;

    /// <summary>
    /// 撞击玩家时的力
    /// </summary>
    public float HitPlayerForce;

    /// <summary>
    /// 目标
    /// </summary>
    [HideInInspector]
    public GameObject Target;

    /// <summary>
    /// 石头的状态
    /// </summary>
    [HideInInspector]
    public RockStates rockState = RockStates.HitPlayer;

    /// <summary>
    /// 朝向
    /// </summary>
    private Vector3 direction;

    /// <summary>
    /// 石头的伤害值，会根据是石头人打击还是玩家反击变更
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
        //TODO:解决石头在石头人手中时，速度为0
        if(rigidbody.velocity.sqrMagnitude < 0.1f)
        {
            rockState = RockStates.HitNothing;
        }
    }

    /// <summary>
    /// 飞向目标
    /// </summary>
    public void FlyToTarget()
    {
        direction = (Target.transform.position - transform.position + Vector3.up).normalized;

        //施加力，类似爆炸力
        rigidbody.AddForce(direction *  Force, ForceMode.Impulse);
    }

    /// <summary>
    /// 碰撞
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {

        //TODO:修改为玩家按下按钮进入防御模式，与按下按钮进行反击
        switch (rockState)
        {
            case RockStates.HitPlayer:
                //在伤害玩家的状态下，如果碰到了玩家，对玩家造成伤害
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
                //反击石头人
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
    /// 放进对象池
    /// </summary>
    /// <returns></returns>
    IEnumerator ReleaseRock()
    {
        yield return new WaitForSeconds(1f);

        ObjectPool.Instance.ReleaseObject(this.name, this.gameObject);
    }
}
