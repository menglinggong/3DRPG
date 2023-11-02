using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 传送门
/// </summary>
public class TransitionPoint : MonoBehaviour
{
    /// <summary>
    /// 传送类型
    /// 同场景传送和异场景传送
    /// </summary>
    public enum TransitionType
    {
        SameScene,
        DiffenceScene
    }

    [Header("传送信息")]
    /// <summary>
    /// 传送到的场景名称
    /// </summary>
    public string SceneName;

    /// <summary>
    /// 传送类型
    /// </summary>
    public TransitionType Type_Transition;

    /// <summary>
    /// 传送终点类型
    /// </summary>
    public TransitionDestination.DestinationType Type_Destination;

    /// <summary>
    /// 是否可以传送
    /// </summary>
    private bool isCanTrans = false;

    private void Awake()
    {
        PortalManager.Instance.AddTransitionPoints(this);
    }

    private void OnDestroy()
    {
        PortalManager.Instance.RemoveTransitionPoints(this);
    }

    /// <summary>
    /// 触发器Stay
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ScenesManager.Instance.TransitionToDestination(this);
        }
            //isCanTrans = true;
    }

    /// <summary>
    /// 触发器离开
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isCanTrans = false;
    }
}
