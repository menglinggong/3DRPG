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
    public float Defense;
    /// <summary>
    /// �;ö�
    /// </summary>
    public float Durability;
    /// <summary>
    /// ��ħЧ��
    /// </summary>
    public ArticleEnchanting Enchant;
    /// <summary>
    /// ��ħ��ֵ
    /// </summary>
    public float EnchantValue;
    /// <summary>
    /// ��������
    /// </summary>
    public ArticleKind_Material MaterialKind;
}
