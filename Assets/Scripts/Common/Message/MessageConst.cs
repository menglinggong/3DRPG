using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息常量
/// </summary>
public partial class MessageConst
{
    public const string UpdateExp = "UpdateExp";
    public const string UpdateHealth = "UpdateHealth";
}

public partial class MessageConst
{
    public class ArticleConst
    {
        /// <summary>
        /// 背包中，物品选中
        /// </summary>
        public const string OnArticleUISelected = "OnArticleUISelected";

        /// <summary>
        /// 显示/隐藏可拾取物品信息的界面
        /// </summary>
        public const string OnShowHideArticleInfo = "OnShowHideArticleInfo";
    }
    
}

public partial class MessageConst
{
    /// <summary>
    /// 输入系统的消息
    /// </summary>
    public class InputSystemConst
    {
        //ABXY按键--对应键盘的UIJK
        public const string OnAPerformed = "OnAPerformed";
        public const string OnBPerformed = "OnBPerformed";
        public const string OnXPerformed = "OnXPerformed";
        public const string OnYPerformed = "OnYPerformed";
        //L，ZL，R，ZR按钮--对应键盘的VBNM
        public const string OnLPerformed = "OnLPerformed";
        public const string OnZLPerformed = "OnZLPerformed";
        public const string OnRerformed = "OnRPerformed";
        public const string OnZRPerformed = "OnZRPerformed";
        //左边的十字按键--对应键盘的ZXCV
        public const string OnUpPerformed = "OnUpPerformed";
        public const string OnDownPerformed = "OnDownPerformed";
        public const string OnLeftPerformed = "OnLeftPerformed";
        public const string OnRightPerformed = "OnRightPerformed";
        //左右遥感--对应键盘的WASD和上下左右按键
        public const string OnLeftStick = "OnLeftStick";
        public const string OnRightStick = "OnRightStick";
        //-+按键--对应键盘的OP
        public const string OnMinusPerformed = "OnMinusPerformed";
        public const string OnPlusPerformed = "OnPlusPerformed";

    }
}
