using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// π≠¿‡ŒÔ∆∑
/// </summary>
public class Article_Bow : Article
{
    [SerializeField]
    private ArticleInfo_Bow articleInfo;

    public override void PickUp()
    {
        ArticleManager.Instance.AddArticle(articleInfo);
        base.PickUp();
    }
}
