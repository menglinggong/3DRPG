using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家界面，包括血条，等级，经验值
/// </summary>
public class PlayerUI : MonoBehaviour
{
    /// <summary>
    /// 等级文本
    /// </summary>
    private Text levelText;
    /// <summary>
    /// 血条
    /// </summary>
    private Image healthImg;
    /// <summary>
    /// 经验值
    /// </summary>
    private Image expImg;
    /// <summary>
    /// 血条材质
    /// </summary>
    private Material healthMaterial;
    /// <summary>
    /// 经验值材质
    /// </summary>
    private Material expMaterial;

    private void Awake()
    {
        levelText = GetComponentInChildren<Text>();
        healthImg = this.transform.Find("PlayerHealthBar").GetComponent<Image>();
        expImg = this.transform.Find("PlayerExpBar").GetComponent<Image>();

        //使用临时创建的材质
        healthMaterial = Instantiate(healthImg.material);
        expMaterial = Instantiate(expImg.material);

        healthImg.material = healthMaterial;
        expImg.material = expMaterial;
        //修改经验值材质的基础设置
        expMaterial.SetFloat("_BlackWidth", 0);
        expMaterial.SetColor("_Color", Color.yellow);
        expMaterial.SetColor("_LossColor", Color.black);
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(MessageConst.UpdateExp, UpdateExp);
        EventManager.Instance.AddListener(MessageConst.UpdateHealth, UpdateHealth);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(MessageConst.UpdateExp, UpdateExp);
        EventManager.Instance.RemoveListener(MessageConst.UpdateHealth, UpdateHealth);
    }

    /// <summary>
    /// 刷新血条
    /// </summary>
    void UpdateHealth(string msg, object value)
    {
        CharacterStats data = value as CharacterStats;
        if (data != GameManager.Instance.PlayerStats) return;

        healthMaterial.SetFloat("_BloodVolume", data.MaxHealth);

        healthMaterial.SetFloat("_life", data.CurrentHealth / data.MaxHealth);
    }

    /// <summary>
    /// 刷新经验条
    /// </summary>
    void UpdateExp(string msg, object value)
    {
        CharacterStats data = value as CharacterStats;
        if (data != GameManager.Instance.PlayerStats) return;

        expMaterial.SetFloat("_BloodVolume", data.CharacterData.BaseExp);
        expMaterial.SetFloat("_life", data.CharacterData.CurrentExp / data.CharacterData.BaseExp);
        levelText.text = data.CharacterData.CurrentLevel.ToString("00");
    }

}
