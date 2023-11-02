using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : ISingleton<GameManager>
{
    /// <summary>
    /// 玩家
    /// </summary>
    [HideInInspector]
    public CharacterStats PlayerStats;

    private CinemachineFreeLook freeLookCamera;

    /// <summary>
    /// 订阅游戏结束的订阅者
    /// </summary>
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    /// <summary>
    /// 在玩家创建时赋值
    /// </summary>
    /// <param name="player"></param>
    public void RigisterPlayer(CharacterStats player)
    {
        PlayerStats = player;

        //设置相机跟踪对象
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        freeLookCamera.Follow = PlayerStats.transform.Find("head");
        freeLookCamera.LookAt = PlayerStats.transform.Find("head");
    }

    /// <summary>
    /// 敌人创建时加入到列表中
    /// </summary>
    /// <param name="obj"></param>
    public void AddEndGameObserver(IEndGameObserver obj)
    {
        endGameObservers.Add(obj);
    }

    /// <summary>
    /// 敌人死亡时移出列表
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveEndGameObserver(IEndGameObserver obj)
    {
        endGameObservers.Remove(obj);
    }

    /// <summary>
    /// 广播游戏结束
    /// </summary>
    public void NotifyObserver()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
}
