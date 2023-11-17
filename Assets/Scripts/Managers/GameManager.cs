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

    private CinemachineFreeLook freeLookCamera;

    /// <summary>
    /// ��ұ�������
    /// </summary>
    [HideInInspector]
    public InventoryUI inventoryUI;

    /// <summary>
    /// ������Ϸ�����Ķ�����
    /// </summary>
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    /// <summary>
    /// ����Ҵ���ʱ��ֵ
    /// </summary>
    /// <param name="player"></param>
    public void RigisterPlayer(CharacterStats player)
    {
        PlayerStats = player;

        //����������ٶ���
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        if (freeLookCamera == null)
            return;

        freeLookCamera.Follow = PlayerStats.transform.Find("head");
        freeLookCamera.LookAt = PlayerStats.transform.Find("head");
    }

    /// <summary>
    /// �����ұ�������
    /// </summary>
    /// <param name="inventoryUI"></param>
    public void RigisterInventoryUI(InventoryUI inventoryUI)
    {
        this.inventoryUI = inventoryUI;
        this.inventoryUI.gameObject.SetActive(false);
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
}
