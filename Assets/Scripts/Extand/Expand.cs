using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��չһЩ��ķ���
/// </summary>
public static class Expand
{

    /// <summary>
    /// Ŀ���ڵ�ǰ��������ķ�Χֵ
    /// </summary>
    private static float dotThreshold = 0.5f;

    /// <summary>
    /// ת��Ŀ�꣬����˲���ת��
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
    /// Ŀ���Ƿ�������һ����Χ��
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="target"></param>
    public static bool IsFacingTarget(this Transform trans, Transform target)
    {
        Vector3 forward = trans.forward.normalized;
        //Ŀ���뵱ǰ���������
        Vector3 dir2 = (target.position - trans.position).normalized;

        return Vector3.Dot(forward, dir2) >= dotThreshold;
    }
}
