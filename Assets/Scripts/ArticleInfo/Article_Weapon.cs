using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器类物品--具体物体模型
/// </summary>
public class Article_Weapon : Article
{
    [SerializeField]
    private ArticleInfo_Weapon articleInfo;

    public override void PickUp()
    {
        InventoryManager.Instance.AddInventoryArticle(articleInfo);
        base.PickUp();
    }
}
