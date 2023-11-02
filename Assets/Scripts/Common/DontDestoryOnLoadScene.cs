using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 切换场景不删除该物体
/// </summary>
public class DontDestoryOnLoadScene : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
