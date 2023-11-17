using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拓展一些类的方法
/// </summary>
public static class Expand
{

    /// <summary>
    /// 目标在当前物体正面的范围值
    /// </summary>
    private static float dotThreshold = 0.5f;

    /// <summary>
    /// 转向目标，不是瞬间的转向
    /// </summary>
    /// <param name="time"></param>
    /// <param name="quaternion"></param>
    /// <returns></returns>
    public static IEnumerator TurnRound(this Transform transform, Vector3 targetPos, float speed)
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        dir = new Vector3(dir.x, 0, dir.z);
        Quaternion rotate = Quaternion.LookRotation(dir);
        while (Vector3.Angle(dir, transform.forward) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, speed);
            yield return null;
        }
    }

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
