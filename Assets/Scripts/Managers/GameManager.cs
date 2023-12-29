using Cinemachine;
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
    /// ��Ϸ�Ƿ���ͣ
    /// </summary>
    private bool isGamePaused;

    public bool IsGamePaused
    {
        get { return isGamePaused; }
    }

    /// <summary>
    /// ������Ϸ�����Ķ�����
    /// </summary>
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    /// <summary>
    /// ����Ҵ���ʱ��ֵ
    /// </summary>
    /// <param name="player"></param>
    public void RegisterPlayer(CharacterStats player)
    {
        PlayerStats = player;
        ArticleManager.Instance.DeserializeArticles();
    }

    /// <summary>
    /// ���˴���ʱ���뵽�б���
    /// </summary>
    /// <param name="obj"></param>
    public void AddEndGameObserver(IEndGameObserver obj)
    {
        endGameObservers.Add(obj);
    }

    /// <summary>
    /// ��������ʱ�Ƴ��б�
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveEndGameObserver(IEndGameObserver obj)
    {
        endGameObservers.Remove(obj);
    }

    /// <summary>
    /// �㲥��Ϸ����
    /// </summary>
    public void NotifyObserver()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }

    /// <summary>
    /// ��ͣ��Ϸ
    /// </summary>
    public void GamePause()
    {
        isGamePaused = true;
        Time.timeScale = 0;
    }

    /// <summary>
    /// ��Ϸ����
    /// </summary>
    public void GameContinue()
    {
        isGamePaused = false;
        Time.timeScale = 1;
    }

    /// <summary>
    /// ���浱ǰ����
    /// </summary>
    public void SaveGame()
    {

    }
}
