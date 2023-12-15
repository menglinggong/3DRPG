using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ʒ
/// </summary>
public class Article_Bow : Article
{
    [SerializeField]
    private ArticleInfo_Bow articleInfo;

    public override void PickUp()
    {
        InventoryManager.Instance.AddInventoryArticle(articleInfo);
        base.PickUp();
    }
}
