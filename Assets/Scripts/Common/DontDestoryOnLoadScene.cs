using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �л�������ɾ��������
/// </summary>
public class DontDestoryOnLoadScene : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
