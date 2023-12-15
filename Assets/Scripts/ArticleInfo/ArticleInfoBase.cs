using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物品的基类，包含所有物品都带有的属性
/// </summary>
[Serializable]
public class ArticleInfoBase
{
    /// <summary>
    /// 物品的id
    /// </summary>
    public int ID;
    /// <summary>
    /// 物品的名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 物品的描述
    /// </summary>
    public string Descrip;
    /// <summary>
    /// 显示图标的路径
    /// </summary>
    public string IconPath;
    /// <summary>
    /// 预制体模型路径
    /// </summary>
    public string PrefabPath;
}

/// <summary>
/// 物品的附魔效果
/// </summary>
public enum ArticleEnchanting
{
    Default,        //无附魔
    AttackUp,       //攻击力增加（比无附魔效果的相同物品拥有更高的攻击力，参照塞尔达）
    DefenceUp,      //防御力增加，同上
    DurabilityUp,   //耐久度增加，同上
}

/// <summary>
/// 物品类型-材质
/// </summary>
public enum ArticleKind_Material
{
    Iron,               //铁制
    Wooden,             //木制
    Stone,              //石制
    Bone,               //骨制
}

