using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ʒ�Ļ��࣬����������Ʒ�����е�����
/// </summary>
[Serializable]
public class ArticleInfoBase
{
    /// <summary>
    /// ��Ʒ��id
    /// </summary>
    public int ID;
    /// <summary>
    /// ��Ʒ������
    /// </summary>
    public string Name;
    /// <summary>
    /// ��Ʒ������
    /// </summary>
    public string Descrip;
    /// <summary>
    /// ��ʾͼ���·��
    /// </summary>
    public string IconPath;
    /// <summary>
    /// Ԥ����ģ��·��
    /// </summary>
    public string PrefabPath;

    public ArticleInfoBase(ArticleInfoBase info)
    {
        this.ID = info.ID;
        this.Name = info.Name;
        this.Descrip = info.Descrip;
        this.IconPath = info.IconPath;
        this.PrefabPath = info.PrefabPath;
    }

    public ArticleInfoBase()
    {

    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    public virtual ArticleInfoBase Copy()
    {
        ArticleInfoBase info = new ArticleInfoBase(this);
        return info;
    }
}

/// <summary>
/// ��Ʒ�ĸ�ħЧ��
/// </summary>
public enum ArticleEnchanting
{
    Default,        //�޸�ħ
    AttackUp,       //���������ӣ����޸�ħЧ������ͬ��Ʒӵ�и��ߵĹ����������������
    DefenceUp,      //���������ӣ�ͬ��
    DurabilityUp,   //�;ö����ӣ�ͬ��
}

/// <summary>
/// ��Ʒ����-����
/// </summary>
public enum ArticleKind_Material
{
    Iron,               //����
    Wooden,             //ľ��
    Stone,              //ʯ��
    Bone,               //����
}

