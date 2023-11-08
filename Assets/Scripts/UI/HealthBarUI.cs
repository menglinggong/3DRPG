using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ѫ��UI
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    /// <summary>
    /// Ѫ��Ԥ����
    /// </summary>
    public GameObject HealthBarPrefab;

    /// <summary>
    /// ��ɫ����
    /// </summary>
    private CharacterStats characterstats;

    /// <summary>
    /// Ѫ����ʾ��
    /// </summary>
    private Transform healthBarPoint;

    /// <summary>
    /// ����������Ѫ��
    /// </summary>
    private GameObject healthBar;

    /// <summary>
    /// Ѫ���Ĳ���
    /// </summary>
    private Material healthBarMaterial;

    /// <summary>
    /// Ѫ���Ļ���
    /// </summary>
    private Transform healthBarCanvas;

    /// <summary>
    /// �����
    /// </summary>
    private Camera mainCamera;

    /// <summary>
    /// �Ƿ�һֱ�ɼ�
    /// </summary>
    public bool IsAlwaysVisble = false;

    /// <summary>
    /// �ɼ�ʱ��
    /// </summary>
    public float VisbleTime = 3;

    /// <summary>
    /// ��ʱ
    /// </summary>
    private float time;

    private void Awake()
    {
        healthBarPoint = this.transform.Find("HealthBarPoint");
        characterstats = this.GetComponent<CharacterStats>();
        healthBarCanvas = GameObject.Find("HealthBarCanvas").transform;
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(MessageConst.UpdateHealth, UpdateHealBar);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(MessageConst.UpdateHealth, UpdateHealBar);
    }

    private void Start()
    {
        //�Ӷ���ػ�ȡѪ��
        healthBar = ObjectPool.Instance.GetObject(HealthBarPrefab.name, HealthBarPrefab);
        //��ʾ�ڻ�����
        healthBar.transform.SetParent(healthBarCanvas, false);

        healthBar.transform.position = healthBarPoint.position;
        healthBar.transform.localScale = Vector3.one;
        healthBar.transform.localRotation = Quaternion.identity;

        //���ò���Ϊ��������
        Material temp = healthBar.transform.GetChild(0).GetComponent<Image>().material;
        healthBarMaterial = Instantiate<Material>(temp);
        healthBar.transform.GetChild(0).GetComponent<Image>().material = healthBarMaterial;
        //������Ѫ��
        healthBarMaterial.SetFloat("_BloodVolume", characterstats.MaxHealth);

        healthBar.SetActive(IsAlwaysVisble);
    }

    private void LateUpdate()
    {
        if (healthBar.activeSelf)
        {
            //Ѫ������0
            RefreshUIPosAndRot();
        }

        if(!IsAlwaysVisble && healthBar.activeSelf)
        {
            if(time <= 0)
            {
                healthBar.SetActive(false);
            }
            else
                time -= Time.deltaTime;
        }
    }

    /// <summary>
    /// ˢ��Ѫ����λ�úͳ���
    /// </summary>
    private void RefreshUIPosAndRot()
    {
        healthBar.transform.position = healthBarPoint.position;

        //healthBar.transform.forward = (mainCamera.transform.position - healthBar.transform.position).normalized;
        healthBar.transform.forward = -mainCamera.transform.forward;
    }

    /// <summary>
    /// ˢ��Ѫ��
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void UpdateHealBar(string msg, object obj)
    {
        CharacterStats stats = (CharacterStats)obj;
        if (stats != characterstats)
            return;


        if (stats.CurrentHealth > 0)
        {
            healthBarMaterial.SetFloat("_life", stats.CurrentHealth / stats.MaxHealth);

            if (!IsAlwaysVisble)
            {
                healthBar.SetActive(true);
                time = VisbleTime;
            }
        }
        else
        {
            //Ѫ��С��0������ػ���
            ObjectPool.Instance.ReleaseObject(HealthBarPrefab.name, healthBar);
        }
    }
}
