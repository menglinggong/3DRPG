using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏结束的接口
/// </summary>
public interface IEndGameObserver
{
    void EndNotify();
}
