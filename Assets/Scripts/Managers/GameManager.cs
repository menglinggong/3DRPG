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

    /// <summary>
    /// 游戏是否暂停
    /// </summary>
    private bool isGamePaused;

    public bool IsGamePaused
    {
        get { return isGamePaused; }
    }

    /// <summary>
    /// 订阅游戏结束的订阅者
    /// </summary>
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    /// <summary>
    /// 在玩家创建时赋值
    /// </summary>
    /// <param name="player"></param>
    public void RegisterPlayer(CharacterStats player)
    {
        PlayerStats = player;
        ArticleManager.Instance.DeserializeArticles();
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

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void GamePause()
    {
        isGamePaused = true;
        Time.timeScale = 0;
    }

    /// <summary>
    /// 游戏继续
    /// </summary>
    public void GameContinue()
    {
        isGamePaused = false;
        Time.timeScale = 1;
    }

    /// <summary>
    /// 保存当前进度
    /// </summary>
    public void SaveGame()
    {

    }
}
