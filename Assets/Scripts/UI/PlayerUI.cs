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

    private void Update()
    {
        if (GameManager.Instance.PlayerStats == null)
            return;

        UpdateHealth();
        UpdateExp();
        UpdateLevel();
    }

    /// <summary>
    /// 刷新血条
    /// </summary>
    void UpdateHealth()
    {
        healthMaterial.SetFloat("_BloodVolume", GameManager.Instance.PlayerStats.MaxHealth);

        healthMaterial.SetFloat("_life", GameManager.Instance.PlayerStats.CurrentHealth / GameManager.Instance.PlayerStats.MaxHealth);
    }

    /// <summary>
    /// 刷新经验条
    /// </summary>
    void UpdateExp()
    {
        expMaterial.SetFloat("_BloodVolume", GameManager.Instance.PlayerStats.CharacterData.BaseExp);

        expMaterial.SetFloat("_life", GameManager.Instance.PlayerStats.CharacterData.CurrentExp / GameManager.Instance.PlayerStats.CharacterData.BaseExp);
    }

    /// <summary>
    /// 刷新等级
    /// </summary>
    void UpdateLevel()
    {
        levelText.text = GameManager.Instance.PlayerStats.CharacterData.CurrentLevel.ToString("00");
    }
}
