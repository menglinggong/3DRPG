using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// 目标
    /// </summary>
    [HideInInspector]
    public GameObject Target;

    /// <summary>
    /// 朝向
    /// </summary>
    private Vector3 direction;

    private void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
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
        StartCoroutine(ReleaseRock());
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
