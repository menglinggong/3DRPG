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
        Enter,
        A,
        B,
        C,
        D
    }

    /// <summary>
    /// �յ�����
    /// </summary>
    public DestinationType Type_Destination;
}
