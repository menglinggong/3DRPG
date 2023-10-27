using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 类方法拓展
/// </summary>
public static class ExtensionMethod
{
    /// <summary>
    /// 目标在当前物体正面的范围值
    /// </summary>
    private static float dotThreshold = 0.5f;

    /// <summary>
    /// 目标是否在正面一定范围内
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="target"></param>
    public static bool IsFacingTarget(this Transform trans, Transform target)
    {
        Vector3 forward = trans.forward.normalized;
        //目标与当前物体的向量
        Vector3 dir2 = (target.position - trans.position).normalized;

        return Vector3.Dot(forward, dir2) >= dotThreshold;
    }
}
