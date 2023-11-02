using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ݵĹ�����
/// </summary>
public class SaveDataManager : ISingleton<SaveDataManager>
{
    /// <summary>
    /// �����������
    /// </summary>
    public void SavePlayerData()
    {
        SaveData("PlayerCharacterData", GameManager.Instance.PlayerStats.CharacterData);
        SaveData("PlayerAttackData", GameManager.Instance.PlayerStats.AttackData);
    }

    /// <summary>
    /// ��ȡ�������
    /// </summary>
    public void LoadPlayerData()
    {
        LoadData("PlayerCharacterData", GameManager.Instance.PlayerStats.CharacterData);
        LoadData("PlayerAttackData", GameManager.Instance.PlayerStats.AttackData);
    }

    /// <summary>
    /// ͨ�ñ�������
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public void SaveData(string key, Object data)
    {
        string jsonData = JsonUtility.ToJson(data, true);
        //���浽���ش���,windows���ڵ�·����ע����ڣ�
        //�����\HKEY_CURRENT_USER\Software\Unity\UnityEditor\[Company Name]\[Product Name]
        //[Company Name]\[Product Name]������unity--Edit--Project Settings--Player����Բ鿴
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ͨ�ö�ȡ����
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
    /// ɾ�����������
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
