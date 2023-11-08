using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// �����ƹ�����
/// </summary>
public class MouseManager : ISingleton<MouseManager>
{
    /// <summary>
    /// �����ͼ���ֱ�Ϊ��ָ�������ţ�������Ŀ��λ�ã���ͷ
    /// </summary>
    public Texture2D Point, Doorway, Attack, Target, Arrow;

    /// <summary>
    /// ������¼�����һ����άλ�õ㣩
    /// </summary>
    public event Action<Vector3> OnMouseClicked;

    /// <summary>
    /// ��������¼�
    /// </summary>
    public event Action<GameObject> OnEnemyClicked;

    /// <summary>
    /// ����
    /// </summary>
    RaycastHit hitInfo;

    private void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    /// <summary>
    /// ���������ʾ��ͼ
    /// </summary>
    private void SetCursorTexture()
    {
        if (Camera.main == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //���������ʾ��ͼ
            switch(hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(Target, new Vector2(16,16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(Attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "AttackAble":
                    Cursor.SetCursor(Attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(Doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
                //case "Point":
                //    break;
                default:
                    Cursor.SetCursor(Arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    /// <summary>
    /// �����ƣ�������棬ִ���¼�
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
            else if(hitInfo.collider.gameObject.CompareTag("Portal"))
                OnMouseClicked?.Invoke(new Vector3(hitInfo.point.x, 0, hitInfo.point.z) + hitInfo.collider.transform.forward * 0.5f);
        }
    }
}

