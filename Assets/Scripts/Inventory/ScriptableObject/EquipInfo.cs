using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// װ����Ϣ
/// </summary>
[CreateAssetMenu(fileName ="New EquipInfo",menuName = "Inventory/New EquipInfo")]
public class EquipInfo : ScriptableObject
{
    /// <summary>
    /// װ������
    /// </summary>
    public string EquipName;
    /// <summary>
    /// װ��ͼ��
    /// </summary>
    public Sprite EquipIcon;
    /// <summary>
    /// װ������
    /// </summary>
    public int EquipsCount;
    /// <summary>
    /// �Ƿ�������Ʒ
    /// </summary>
    public bool IsConsumables;
    /// <summary>
    /// װ��������Ϣ
    /// </summary>
    [TextArea]
    public string EquipDescribe;
}
