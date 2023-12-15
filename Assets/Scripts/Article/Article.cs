using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品
/// </summary>
public class Article : MonoBehaviour
{
    /// <summary>
    /// 是否手持
    /// </summary>
    private bool isInHand = false;

    /// <summary>
    /// 是否在宝箱中
    /// </summary>
    private bool isInBox = false;

    /// <summary>
    /// 存在时长
    /// </summary>
    [SerializeField]
    private float lifeTime;

    /// <summary>
    /// 计时
    /// </summary>
    private float time;

    /// <summary>
    /// 显示可拾取的半径
    /// </summary>
    [SerializeField]
    private float radio = 3;

    /// <summary>
    /// 玩家
    /// </summary>
    private Transform player;

    private void OnEnable()
    {
        time = lifeTime;
    }

    private void Update()
    {
        if (isInHand)
            return;

        if(!isInBox)
        {
            if (time <= 0)
                ObjectPool.Instance.ReleaseObject(this.gameObject.name, this.gameObject);
            else
                time -= Time.deltaTime;

            if(IsPlayerInRadio())
            {
                //TODO，实现屏幕拾取界面
                Debug.Log("拾取！！！");
                if(Input.GetKeyDown(KeyCode.A))
                {
                    PickUp();
                }
            }
        }
        else
        {
            if (IsPlayerInRadio())
            {
                Debug.Log("打开！！！");
            }
        }

    }

    /// <summary>
    /// 玩家是否在可拾取物品的范围内
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerInRadio()
    {
        if (GameManager.Instance.PlayerStats != null)
        {
            player = GameManager.Instance.PlayerStats.transform;

            float dis = Vector3.Distance(player.position, this.transform.position);

            return dis < radio;
        }
        
        return false;
    }

    /// <summary>
    /// 拾取物品的方法
    /// </summary>
    public virtual void PickUp()
    {
        isInHand = true;
        ObjectPool.Instance.ReleaseObject(this.gameObject.name, this.gameObject);
    }
}
