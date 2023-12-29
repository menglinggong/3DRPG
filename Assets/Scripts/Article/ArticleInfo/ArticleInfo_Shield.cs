using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������Ʒ����Ϣ
/// </summary>
[Serializable]
public class ArticleInfo_Shield : ArticleInfoBase
{
    /// <summary>
    /// ������
    /// </summary>
    public int Defense;
    /// <summary>
    /// �;ö�
    /// </summary>
    public int Durability;
    /// <summary>
    /// ��ħЧ��
    /// </summary>
    public ArticleEnchanting Enchant;
    /// <summary>
    /// ��ħ��ֵ
    /// </summary>
    public int EnchantValue;
    /// <summary>
    /// ��������
    /// </summary>
    public ArticleKind_Material MaterialKind;

    public ArticleInfo_Shield(ArticleInfo_Shield info) : base(info)
    {
        this.Defense = info.Defense;
        this.Durability = info.Durability;
        this.Enchant = info.Enchant;
        this.EnchantValue = info.EnchantValue;
        this.MaterialKind = info.MaterialKind;
    }

    public ArticleInfo_Shield()
    {

    }

    public override ArticleInfoBase Copy()
    {
        ArticleInfo_Shield info = new ArticleInfo_Shield(this);
        return info;
    }
}
