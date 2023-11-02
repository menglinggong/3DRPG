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
        Not,
        Scene01_A,
        Scene01_B,
        Scene01_C,
        Scene01_D,
        Scene02_A,
        Scene02_B,
        Scene02_C,
    }

    /// <summary>
    /// 终点类型
    /// </summary>
    public DestinationType Type_Destination;
}
