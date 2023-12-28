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
}
