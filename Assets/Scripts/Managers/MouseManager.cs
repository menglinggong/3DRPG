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
public class MouseManager : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    private static MouseManager instance;

    /// <summary>
    /// ��������
    /// </summary>
    public static MouseManager Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// �����ͼ���ֱ�Ϊ��ָ�������ţ�������Ŀ��λ�ã���ͷ
    /// </summary>
    public Texture2D Point, Doorway, Attack, Target, Arrow;

    /// <summary>
    /// ������¼�����һ����άλ�õ㣩
    /// </summary>
    public event Action<Vector3> OnMouseClicked;

    /// <summary>
    /// ����
    /// </summary>
    RaycastHit hitInfo;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);

        instance = this;
    }

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
    /// �����ƣ�������棬ִ���¼�
    /// </summary>
    private void MouseControl()
    {
        if(Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
                OnMouseClicked?.Invoke(hitInfo.point);
        }
    }
}

