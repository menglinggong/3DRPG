using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������Ʒ
/// </summary>
public class Article_Shield : Article
{
    [SerializeField]
    private ArticleInfo_Shield articleInfo;

    public override void PickUp()
    {
        InventoryManager.Instance.AddInventoryArticle(articleInfo);
        base.PickUp();
    }
}
