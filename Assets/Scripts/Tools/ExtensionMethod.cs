using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �෽����չ
/// </summary>
public static class ExtensionMethod
{
    /// <summary>
    /// Ŀ���ڵ�ǰ��������ķ�Χֵ
    /// </summary>
    private static float dotThreshold = 0.5f;

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
