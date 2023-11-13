using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装备信息
/// </summary>
[CreateAssetMenu(fileName ="New EquipInfo",menuName = "Inventory/New EquipInfo")]
public class EquipInfo : ScriptableObject
{
    /// <summary>
    /// 装备名称
    /// </summary>
    public string EquipName;
    /// <summary>
    /// 装备图标
    /// </summary>
    public Sprite EquipIcon;
    /// <summary>
    /// 装备数量
    /// </summary>
    public int EquipsCount;
    /// <summary>
    /// 是否是消耗品
    /// </summary>
    public bool IsConsumables;
    /// <summary>
    /// 装备描述信息
    /// </summary>
    [TextArea]
    public string EquipDescribe;
}
