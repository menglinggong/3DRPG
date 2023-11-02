using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 传送门管理器
/// </summary>
public class PortalManager : ISingleton<PortalManager>
{
    private List<TransitionPoint> transitionPoints = new List<TransitionPoint>();

    private List<TransitionDestination> transitionDestinations = new List<TransitionDestination>();

    public void AddTransitionPoints(TransitionPoint transitionPoint)
    {
        transitionPoints.Add(transitionPoint);
        transitionDestinations.Add(transitionPoint.GetComponentInChildren<TransitionDestination>());
    }

    public TransitionDestination GetTransitionDestinationByType(TransitionDestination.DestinationType destinationType)
    {
        foreach (TransitionDestination destination in transitionDestinations)
        {
            if(destination.Type_Destination == destinationType)
            {
                return destination;
            }
        }

        return null;
    }
}
