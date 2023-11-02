using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������յ�
/// </summary>
public class TransitionDestination : MonoBehaviour
{
    /// <summary>
    /// �յ�����
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
    /// �յ�����
    /// </summary>
    public DestinationType Type_Destination;
}
