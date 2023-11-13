using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ±³°ü
/// </summary>
[CreateAssetMenu(fileName = "New InventoryInfo", menuName = "Inventory/New InventoryInfo")]
public class InventoryInfo : ScriptableObject
{
    public string InventoryName;
    public List<EquipInfo> EquipInfoList = new List<EquipInfo>();
    
}
