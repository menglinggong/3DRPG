using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重要物品的信息
/// </summary>
public class ArticleInfo_Import : ArticleInfoBase
{
    public ArticleInfo_Import(ArticleInfoBase info) : base(info)
    {
    }

    //TODO，暂时不清楚怎么设计
    public override ArticleInfoBase Copy()
    {
        return null;
    }
}
