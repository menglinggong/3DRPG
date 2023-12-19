using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEvent : UnityEvent<string, object> { }

/// <summary>
/// 消息管理器
/// </summary>
public class EventManager : ISingleton<EventManager>
{
    /// <summary>
    /// 存放所有消息的字典
    /// </summary>
    private Dictionary<string, CustomEvent> golbalEventDictionary = new Dictionary<string, CustomEvent>();

    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void AddListener(string eventName, UnityAction<string, object> listener)
    {
        CustomEvent thisEvent = null;
        if(!golbalEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent = new CustomEvent();
            golbalEventDictionary.Add(eventName, thisEvent);
        }
        thisEvent.AddListener(listener);
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void RemoveListener(string eventName, UnityAction<string, object> listener)
    {
        CustomEvent thisEvent = null;
        if(golbalEventDictionary.TryGetValue(eventName, out thisEvent))
            thisEvent.RemoveListener(listener);
    }

    /// <summary>
    /// 移除所有监听
    /// </summary>
    /// <param name="eventName"></param>
    public void RemoveAllListener(string eventName)
    {
        CustomEvent thisEvent = null;
        if (golbalEventDictionary.TryGetValue(eventName, out thisEvent))
            thisEvent.RemoveAllListeners();
    }

    /// <summary>
    /// 执行监听
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventParams"></param>
    public void Invoke(string eventName, object eventParams)
    {
        CustomEvent thisEvent = null;
        if (golbalEventDictionary.TryGetValue(eventName, out thisEvent))
            thisEvent.Invoke(eventName, eventParams);
    }

    protected override void OnDestory()
    {
        base.OnDestory();
        golbalEventDictionary.Clear();
    }
}
