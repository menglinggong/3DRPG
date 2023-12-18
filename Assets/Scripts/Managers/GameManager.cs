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
    /// 玩家背包界面
    /// </summary>
    [HideInInspector]
    public InventoryUI InventoryUI;

    /// <summary>
    /// 显示可拾取物品信息的界面
    /// </summary>
    [HideInInspector]
    public ArticleInfoUI ArticleInfoUI;

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

        //设置相机跟踪对象
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        if (freeLookCamera == null)
            return;

        freeLookCamera.Follow = PlayerStats.transform.Find("head");
        freeLookCamera.LookAt = PlayerStats.transform.Find("head");
    }

    /// <summary>
    /// 添加玩家背包界面
    /// </summary>
    /// <param name="inventoryUI"></param>
    public void RegisterInventoryUI(InventoryUI inventoryUI)
    {
        this.InventoryUI = inventoryUI;
        this.InventoryUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 注册物品信息界面
    /// </summary>
    /// <param name="articleInfoUI"></param>
    public void RegisterArticleInfoUI(ArticleInfoUI articleInfoUI)
    {
        this.ArticleInfoUI = articleInfoUI;
        this.ArticleInfoUI.HideArticleInfo();
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
