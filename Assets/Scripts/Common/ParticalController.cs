using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 粒子特效控制器
/// </summary>
public class ParticalController : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        //移入对象池
        ObjectPool.Instance.ReleaseObject(this.name, this.gameObject);
    }
}
