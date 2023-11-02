using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ź�����
/// </summary>
public class PortalManager : ISingleton<PortalManager>
{
    /// <summary>
    /// �������б�
    /// </summary>
    private List<TransitionPoint> transitionPoints = new List<TransitionPoint>();

    /// <summary>
    /// �������յ��б�
    /// </summary>
    private List<TransitionDestination> transitionDestinations = new List<TransitionDestination>();

    /// <summary>
    /// ��Ӵ�����
    /// </summary>
    /// <param name="transitionPoint"></param>
    public void AddTransitionPoints(TransitionPoint transitionPoint)
    {
        transitionPoints.Add(transitionPoint);
        transitionDestinations.Add(transitionPoint.GetComponentInChildren<TransitionDestination>());
    }

    /// <summary>
    /// �Ƴ�������
    /// </summary>
    /// <param name="transitionPoint"></param>
    public void RemoveTransitionPoints(TransitionPoint transitionPoint)
    {
        transitionPoints.Remove(transitionPoint);
        transitionDestinations.Remove(transitionPoint.GetComponentInChildren<TransitionDestination>());
    }

    /// <summary>
    /// ��ȡָ���յ����͵Ĵ������յ�
    /// </summary>
    /// <param name="destinationType"></param>
    /// <returns></returns>
    public TransitionDestination GetTransitionDestinationByType(TransitionDestination.DestinationType destinationType)
    {
        foreach (TransitionDestination destination in transitionDestinations)
        {
            if (destination.Type_Destination == destinationType)
            {
                return destination;
            }
        }

        return null;
    }
}
