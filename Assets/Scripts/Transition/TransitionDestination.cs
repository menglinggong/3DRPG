using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 传送门终点
/// </summary>
public class TransitionDestination : MonoBehaviour
{
    /// <summary>
    /// 终点类型
    /// </summary>
    public enum DestinationType
    {
        Enter,
        A,
        B,
        C,
        D
    }

    /// <summary>
    /// 终点类型
    /// </summary>
    public DestinationType Type_Destination;
}
