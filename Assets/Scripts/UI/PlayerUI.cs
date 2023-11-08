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
    /// ˢ��Ѫ��
    /// </summary>
    void UpdateHealth(string msg, object value)
    {
        CharacterStats data = value as CharacterStats;
        if (data != GameManager.Instance.PlayerStats) return;

        healthMaterial.SetFloat("_BloodVolume", data.MaxHealth);

        healthMaterial.SetFloat("_life", data.CurrentHealth / data.MaxHealth);
    }

    /// <summary>
    /// ˢ�¾�����
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
