using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 传送门管理器
/// </summary>
public class PortalManager : ISingleton<PortalManager>
{
    /// <summary>
    /// 传送门列表
    /// </summary>
    private List<TransitionPoint> transitionPoints = new List<TransitionPoint>();

    /// <summary>
    /// 传送门终点列表
    /// </summary>
    private List<TransitionDestination> transitionDestinations = new List<TransitionDestination>();

    /// <summary>
    /// 添加传送门
    /// </summary>
    /// <param name="transitionPoint"></param>
    public void AddTransitionPoints(TransitionPoint transitionPoint)
    {
        transitionPoints.Add(transitionPoint);
        transitionDestinations.Add(transitionPoint.GetComponentInChildren<TransitionDestination>());
    }

    /// <summary>
    /// 移除传送门
    /// </summary>
    /// <param name="transitionPoint"></param>
    public void RemoveTransitionPoints(TransitionPoint transitionPoint)
    {
        transitionPoints.Remove(transitionPoint);
        transitionDestinations.Remove(transitionPoint.GetComponentInChildren<TransitionDestination>());
    }

    /// <summary>
    /// 获取指定终点类型的传送门终点
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
