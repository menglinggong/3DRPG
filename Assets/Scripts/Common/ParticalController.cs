using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ч������
/// </summary>
public class ParticalController : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        //��������
        ObjectPool.Instance.ReleaseObject(this.name, this.gameObject);
    }
}
