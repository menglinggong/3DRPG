using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ������
/// </summary>
public class GameManager : ISingleton<GameManager>
{
    /// <summary>
    /// ���
    /// </summary>
    [HideInInspector]
    public CharacterStats PlayerStats;

    /// <summary>
    /// ����Ҵ���ʱ��ֵ
    /// </summary>
    /// <param name="player"></param>
    public void RigisterPlayer(CharacterStats player)
    {
        PlayerStats = player;
    }
}
