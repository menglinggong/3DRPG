using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���н���Ļ��������ý���Ԫ��ѡ��
/// </summary>
public class UIBase : MonoBehaviour 
{
    /// <summary>
    /// ��ǰѡ�е�Ԫ��
    /// </summary>
    [HideInInspector]
    public GameObject CurrentSelectedObj;
    /// <summary>
    /// Ĭ��ѡ�е�Ԫ��
    /// </summary>
    [HideInInspector]
    public GameObject DefaultSelectedObj;

    /// <summary>
    /// �����
    /// </summary>
    public virtual void OnShow()
    {
        if (CurrentSelectedObj == null)
            CurrentSelectedObj = DefaultSelectedObj;

        InputSystemManager.Instance.SetCurrentSelectedObj(CurrentSelectedObj);
        InputSystemManager.Instance.SetLastSelectedObj(CurrentSelectedObj);
    }

    /// <summary>
    /// ����ر�
    /// </summary>
    public virtual void OnHide()
    {
        CurrentSelectedObj = InputSystemManager.Instance.GetCurrentSelectedObj();
        InputSystemManager.Instance.SetLastSelectedObj(null);
    }
}
