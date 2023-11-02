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

    private void Update()
    {
        if (GameManager.Instance.PlayerStats == null)
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
        healthMaterial.SetFloat("_BloodVolume", GameManager.Instance.PlayerStats.MaxHealth);

        healthMaterial.SetFloat("_life", GameManager.Instance.PlayerStats.CurrentHealth / GameManager.Instance.PlayerStats.MaxHealth);
    }

    /// <summary>
    /// ˢ�¾�����
    /// </summary>
    void UpdateExp()
    {
        expMaterial.SetFloat("_BloodVolume", GameManager.Instance.PlayerStats.CharacterData.BaseExp);

        expMaterial.SetFloat("_life", GameManager.Instance.PlayerStats.CharacterData.CurrentExp / GameManager.Instance.PlayerStats.CharacterData.BaseExp);
    }

    /// <summary>
    /// ˢ�µȼ�
    /// </summary>
    void UpdateLevel()
    {
        levelText.text = GameManager.Instance.PlayerStats.CharacterData.CurrentLevel.ToString("00");
    }
}
