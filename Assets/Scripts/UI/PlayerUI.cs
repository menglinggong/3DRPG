using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ҽ��棬����Ѫ�����ȼ�������ֵ
/// </summary>
public class PlayerUI : MonoBehaviour
{
    /// <summary>
    /// �ȼ��ı�
    /// </summary>
    private Text levelText;
    /// <summary>
    /// Ѫ��
    /// </summary>
    private Image healthImg;
    /// <summary>
    /// ����ֵ
    /// </summary>
    private Image expImg;
    /// <summary>
    /// Ѫ������
    /// </summary>
    private Material healthMaterial;
    /// <summary>
    /// ����ֵ����
    /// </summary>
    private Material expMaterial;
    /// <summary>
    /// �������
    /// </summary>
    private CharacterStats playerStats;

    private void Awake()
    {
        levelText = GetComponentInChildren<Text>();
        healthImg = this.transform.Find("PlayerHealthBar").GetComponent<Image>();
        expImg = this.transform.Find("PlayerExpBar").GetComponent<Image>();

        //ʹ����ʱ�����Ĳ���
        healthMaterial = Instantiate(healthImg.material);
        expMaterial = Instantiate(expImg.material);

        healthImg.material = healthMaterial;
        expImg.material = expMaterial;
        //�޸ľ���ֵ���ʵĻ�������
        expMaterial.SetFloat("_BlackWidth", 0);
        expMaterial.SetColor("_Color", Color.yellow);
        expMaterial.SetColor("_LossColor", Color.black);
    }

    private void Start()
    {
        playerStats = GameManager.Instance.PlayerStats;
    }

    private void Update()
    {
        if (playerStats == null)
            return;

        UpdateHealth();
        UpdateExp();
        UpdateLevel();
    }

    /// <summary>
    /// ˢ��Ѫ��
    /// </summary>
    void UpdateHealth()
    {
        healthMaterial.SetFloat("_BloodVolume", playerStats.MaxHealth);

        healthMaterial.SetFloat("_life", playerStats.CurrentHealth / playerStats.MaxHealth);
    }

    /// <summary>
    /// ˢ�¾�����
    /// </summary>
    void UpdateExp()
    {
        expMaterial.SetFloat("_BloodVolume", playerStats.CharacterData.BaseExp);

        expMaterial.SetFloat("_life", playerStats.CharacterData.CurrentExp / playerStats.CharacterData.BaseExp);
    }

    /// <summary>
    /// ˢ�µȼ�
    /// </summary>
    void UpdateLevel()
    {
        levelText.text = playerStats.CharacterData.CurrentLevel.ToString("00");
    }
}
