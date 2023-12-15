using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������Ʒ--��������ģ��
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
