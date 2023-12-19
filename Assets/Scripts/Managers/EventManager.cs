using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEvent : UnityEvent<string, object> { }

/// <summary>
/// ��Ϣ������
/// </summary>
public class EventManager : ISingleton<EventManager>
{
    /// <summary>
    /// ���������Ϣ���ֵ�
    /// </summary>
    private Dictionary<string, CustomEvent> golbalEventDictionary = new Dictionary<string, CustomEvent>();

    /// <summary>
    /// ��Ӽ���
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
    /// �Ƴ�����
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
    /// �Ƴ����м���
    /// </summary>
    /// <param name="eventName"></param>
    public void RemoveAllListener(string eventName)
    {
        CustomEvent thisEvent = null;
        if (golbalEventDictionary.TryGetValue(eventName, out thisEvent))
            thisEvent.RemoveAllListeners();
    }

    /// <summary>
    /// ִ�м���
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
