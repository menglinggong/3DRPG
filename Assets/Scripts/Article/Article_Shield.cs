using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ∂‹≈∆¿‡ŒÔ∆∑
/// </summary>
public class Article_Shield : Article
{
    [SerializeField]
    private ArticleInfo_Shield articleInfo;

    public override void InitInfoBase()
    {
        base.infoBase = articleInfo;
    }

    public override void PickUp()
    {
        ArticleManager.Instance.PickUpArticle(articleInfo, this);
    }
}
