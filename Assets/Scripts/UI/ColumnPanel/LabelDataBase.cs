using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ͬ�������ݵĻ���
/// </summary>
public class LabelDataBase : MonoBehaviour
{
    public virtual void ShowHide(bool isShow)
    {
        this.gameObject.SetActive(isShow);
    }
}
