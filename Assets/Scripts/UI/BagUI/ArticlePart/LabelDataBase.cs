using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ͬ�������ݵĻ���
/// </summary>
public class LabelDataBase : MonoBehaviour
{
    /// <summary>
    /// ѡ�и��ӵ�����
    /// </summary>
    protected int selectedIndex_X = 0;
    protected int selectedIndex_Y = 0;
    /// <summary>
    /// ��ǰ��ҳ�±�
    /// </summary>
    protected int selectedPageIndex = 0;

    /// <summary>
    /// �����ܴ�ŵĸ�������
    /// </summary>
    public Vector2 rowColumn = Vector2.zero;

    public virtual void ShowHide(bool isShow)
    {
        this.gameObject.SetActive(isShow);
    }

    public virtual void AddListener()
    {

    }

    public virtual void RemoveListener()
    {

    }
}

/// <summary>
/// ��Ʒ����---����Ʒ��������һ��
/// </summary>
public enum ArticleInfoType
{
    ArticleInfo_Bow,
    ArticleInfo_Cloth,
    ArticleInfo_EndProduct,
    ArticleInfo_Import,
    ArticleInfo_Shield,
    ArticleInfo_SourceMaterial,
    ArticleInfo_Weapon,
}
