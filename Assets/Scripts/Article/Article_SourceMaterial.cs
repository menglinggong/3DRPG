using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Àÿ≤ƒ¿‡ŒÔ∆∑
/// </summary>
public class Article_SourceMaterial : Article
{
    [SerializeField]
    private ArticleInfo_SourceMaterial articleInfo;

    public override void InitInfoBase()
    {
        base.infoBase = articleInfo;
    }

    public override void PickUp()
    {
        ArticleManager.Instance.PickUpArticle(articleInfo, this, true);
    }
}
