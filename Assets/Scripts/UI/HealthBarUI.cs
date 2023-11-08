using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 血条UI
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    /// <summary>
    /// 血条预制体
    /// </summary>
    public GameObject HealthBarPrefab;

    /// <summary>
    /// 角色数据
    /// </summary>
    private CharacterStats characterstats;

    /// <summary>
    /// 血条显示点
    /// </summary>
    private Transform healthBarPoint;

    /// <summary>
    /// 创建出来的血条
    /// </summary>
    private GameObject healthBar;

    /// <summary>
    /// 血条的材质
    /// </summary>
    private Material healthBarMaterial;

    /// <summary>
    /// 血条的画布
    /// </summary>
    private Transform healthBarCanvas;

    /// <summary>
    /// 主相机
    /// </summary>
    private Camera mainCamera;

    /// <summary>
    /// 是否一直可见
    /// </summary>
    public bool IsAlwaysVisble = false;

    /// <summary>
    /// 可见时间
    /// </summary>
    public float VisbleTime = 3;

    /// <summary>
    /// 计时
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
        //从对象池获取血条
        healthBar = ObjectPool.Instance.GetObject(HealthBarPrefab.name, HealthBarPrefab);
        //显示在画布上
        healthBar.transform.SetParent(healthBarCanvas, false);

        healthBar.transform.position = healthBarPoint.position;
        healthBar.transform.localScale = Vector3.one;
        healthBar.transform.localRotation = Quaternion.identity;

        //设置材质为独立材质
        Material temp = healthBar.transform.GetChild(0).GetComponent<Image>().material;
        healthBarMaterial = Instantiate<Material>(temp);
        healthBar.transform.GetChild(0).GetComponent<Image>().material = healthBarMaterial;
        //设置总血量
        healthBarMaterial.SetFloat("_BloodVolume", characterstats.MaxHealth);

        healthBar.SetActive(IsAlwaysVisble);
    }

    private void LateUpdate()
    {
        if (healthBar.activeSelf)
        {
            //血条大于0
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
    /// 刷新血条的位置和朝向
    /// </summary>
    private void RefreshUIPosAndRot()
    {
        healthBar.transform.position = healthBarPoint.position;

        //healthBar.transform.forward = (mainCamera.transform.position - healthBar.transform.position).normalized;
        healthBar.transform.forward = -mainCamera.transform.forward;
    }

    /// <summary>
    /// 刷新血条
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
            //血条小于0，对象池回收
            ObjectPool.Instance.ReleaseObject(HealthBarPrefab.name, healthBar);
        }
    }
}
