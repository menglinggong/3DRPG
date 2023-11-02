using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������
/// </summary>
public class TransitionPoint : MonoBehaviour
{
    /// <summary>
    /// ��������
    /// ͬ�������ͺ��쳡������
    /// </summary>
    public enum TransitionType
    {
        SameScene,
        DiffenceScene
    }

    [Header("������Ϣ")]
    /// <summary>
    /// ���͵��ĳ�������
    /// </summary>
    public string SceneName;

    /// <summary>
    /// ��������
    /// </summary>
    public TransitionType Type_Transition;

    /// <summary>
    /// �����յ�����
    /// </summary>
    public TransitionDestination.DestinationType Type_Destination;

    /// <summary>
    /// �Ƿ���Դ���
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
    /// ������Stay
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
    /// �������뿪
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isCanTrans = false;
    }
}
