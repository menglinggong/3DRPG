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
    /// 在玩家创建时赋值
    /// </summary>
    /// <param name="player"></param>
    public void RigisterPlayer(CharacterStats player)
    {
        PlayerStats = player;
    }
}
