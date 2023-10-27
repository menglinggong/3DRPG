using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// Ŀ��
    /// </summary>
    [HideInInspector]
    public GameObject Target;

    /// <summary>
    /// ����
    /// </summary>
    private Vector3 direction;

    private void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
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
        StartCoroutine(ReleaseRock());
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
