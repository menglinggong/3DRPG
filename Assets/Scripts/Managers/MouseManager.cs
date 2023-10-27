using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 鼠标控制管理器
/// </summary>
public class MouseManager : ISingleton<MouseManager>
{
    /// <summary>
    /// 鼠标贴图，分别为手指，传送门，攻击，目标位置，箭头
    /// </summary>
    public Texture2D Point, Doorway, Attack, Target, Arrow;

    /// <summary>
    /// 鼠标点击事件（传一个三维位置点）
    /// </summary>
    public event Action<Vector3> OnMouseClicked;

    /// <summary>
    /// 点击敌人事件
    /// </summary>
    public event Action<GameObject> OnEnemyClicked;

    /// <summary>
    /// 射线
    /// </summary>
    RaycastHit hitInfo;

    private void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    /// <summary>
    /// 设置鼠标显示贴图
    /// </summary>
    private void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //设置鼠标显示贴图
            switch(hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(Target, new Vector2(16,16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(Attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                    //case "Doorway":
                    //    break;
                    //case "Point":
                    //    break;
                    //default:
                    //    break;
            }
        }
    }

    /// <summary>
    /// 鼠标控制，点击地面，执行事件
    /// </summary>
    private void MouseControl()
    {
        if(Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
                OnMouseClicked?.Invoke(hitInfo.point);
            else if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            else if (hitInfo.collider.gameObject.CompareTag("AttackAble"))
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
        }
    }
}

