using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保存数据的管理器
/// </summary>
public class SaveDataManager : ISingleton<SaveDataManager>
{
    /// <summary>
    /// 保存玩家数据
    /// </summary>
    public void SavePlayerData()
    {
        SaveData("PlayerCharacterData", GameManager.Instance.PlayerStats.CharacterData);
        SaveData("PlayerAttackData", GameManager.Instance.PlayerStats.AttackData);
    }

    /// <summary>
    /// 读取玩家数据
    /// </summary>
    public void LoadPlayerData()
    {
        LoadData("PlayerCharacterData", GameManager.Instance.PlayerStats.CharacterData);
        LoadData("PlayerAttackData", GameManager.Instance.PlayerStats.AttackData);
    }

    /// <summary>
    /// 通用保存数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public void SaveData(string key, Object data)
    {
        string jsonData = JsonUtility.ToJson(data, true);
        //保存到本地磁盘,windows存在的路径在注册表内，
        //计算机\HKEY_CURRENT_USER\Software\Unity\UnityEditor\[Company Name]\[Product Name]
        //[Company Name]\[Product Name]可以在unity--Edit--Project Settings--Player里可以查看
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 通用读取数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public void LoadData(string key, Object data)
    {
        if(PlayerPrefs.HasKey(key))
        {
            string jsonData = PlayerPrefs.GetString(key);
            JsonUtility.FromJsonOverwrite(jsonData, data);
        }
    }

    /// <summary>
    /// 删除保存的数据
    /// </summary>
    /// <param name="key"></param>
    public void DeleteData(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
