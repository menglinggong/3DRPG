using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 背包-物品功能按钮（装备，卸下，食用，手持等）
/// </summary>
public class Bag_ArticleBtns : MonoBehaviour
{
    [Header("所有按钮")]
    [SerializeField]
    private List<ArticleBtn> articleBtns = new List<ArticleBtn>();

    /// <summary>
    /// 显示的按钮
    /// </summary>
    private List<ArticleBtn> showBtns = new List<ArticleBtn>();

    /// <summary>
    /// 当前选中的按钮
    /// </summary>
    private ArticleBtn currentSelectedBtn;

    /// <summary>
    /// 当前选中的按钮的下标
    /// </summary>
    private int currentIndex = 0;

    /// <summary>
    /// 接收键盘输入的反应时间
    /// </summary>
    private float reactionTime = 1f;

    /// <summary>
    /// 计时
    /// </summary>
    private float timeCount = 0;

    private UnityAction onPanelClose;

    public UnityAction OnPanelClose
    {
        get { return onPanelClose; }
        set { onPanelClose = value; }
    }

    #region 生命周期函数

    private void Awake()
    {
        foreach (var item in articleBtns)
        {
            item.OnValueChanged += (btn, isOn) =>
            {
                if (isOn)
                    currentSelectedBtn = btn;
            };
        }
    }

    private void Update()
    {
        if (timeCount > 0)
        {
            timeCount -= Time.fixedUnscaledDeltaTime;
        }
    }

    #endregion

    #region 输入监听

    /// <summary>
    /// 确认
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnConfirm(string messageConst, object data)
    {
        //TODO根据当前选中的按钮，执行对应方法
        currentSelectedBtn.onBtnClick?.Invoke();
    }

    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnCancel(string messageConst, object data)
    {
        Hide();
    }

    /// <summary>
    /// 切换选中按钮
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnSwitch(string messageConst, object data)
    {
        Vector2 offset = (Vector2)data;
        if (offset.y == 0 || timeCount > 0) return;

        timeCount = reactionTime;

        currentIndex = Mathf.CeilToInt(currentIndex - offset.y);
        currentIndex = Mathf.Clamp(currentIndex, 0, showBtns.Count - 1);

        showBtns[currentIndex].SetSelected(true);
    }

    #endregion

    #region 内部方法

    

    /// <summary>
    /// 显示默认的按钮
    /// </summary>
    /// <param name="id"></param>
    private void ShowDefaultBtns(int id)
    {
        int key = id / 10000;
        showBtns.Clear();
        foreach (var item in articleBtns)
        {
            if(item.CheckSupport(key))
            {
                //TODO判断是否为武器，弓，盾牌，服饰类，若是需要根据玩家是否已经装备进行调整

                showBtns.Add(item);
                item.Show();
            }
            else
                item.Hide();
        }

        if (showBtns.Count > 0)
        {
            showBtns[0].SetSelected(true);
            currentIndex = 0;
        }
    }

    /// <summary>
    /// 添加输入监听
    /// </summary>
    private void AddListener()
    {
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnAPerformed, OnConfirm);
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnBPerformed, OnCancel);
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnLeftStick, OnSwitch);
    }

    /// <summary>
    /// 移除输入的监听
    /// </summary>
    private void RemoveListener()
    {
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnAPerformed, OnConfirm);
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnBPerformed, OnCancel);
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnLeftStick, OnSwitch);
    }

    #endregion

    #region 外部方法

    /// <summary>
    /// 打开界面
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);
        AddListener();
        ShowDefaultBtns(ArticleManager.Instance.CurrentArticle.ID);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public void Hide()
    {
        RemoveListener();
        onPanelClose?.Invoke();
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 装备
    /// </summary>
    public void OnEquipBtnClick()
    {
        Debug.Log("装备");
        ArticleManager.Instance.EquipArticle(ArticleManager.Instance.CurrentArticle);
    }

    /// <summary>
    /// 卸下
    /// </summary>
    public void OnDisboardBtnClick()
    {
        Debug.Log("卸下");
        ArticleManager.Instance.DisboardArticle(ArticleManager.Instance.CurrentArticle);
    }

    /// <summary>
    /// 丢弃
    /// </summary>
    public void OnDiscardBtnClick()
    {
        Debug.Log("丢弃");
        ArticleManager.Instance.DiscardArticle(new List<ArticleInfoBase>() { ArticleManager.Instance.CurrentArticle });
    }

    /// <summary>
    /// 食用
    /// </summary>
    public void OnEatBtnClick()
    {
        //修改数据库数据
        ArticleManager.Instance.UseArticle(ArticleManager.Instance.CurrentArticle);
        //刷新物品格子
        ArticleManager.Instance.CurrentItemFram.Refresh();
        //TODO实现物品的效果
        Hide();
    }

    /// <summary>
    /// 手持
    /// </summary>
    public void OnHandBtnClick()
    {
        Debug.Log("手持");
        ArticleManager.Instance.HoldArticle(new List<ArticleInfoBase>() { ArticleManager.Instance.CurrentArticle });
    }

    /// <summary>
    /// 查看
    /// </summary>
    public void OnCheckBtnClick()
    {
        Debug.Log("查看");
        ArticleManager.Instance.CheckArticle(ArticleManager.Instance.CurrentArticle);
    }

    /// <summary>
    /// 取消
    /// </summary>
    public void OnCanelBtnClick()
    {
        Hide();
    }

    #endregion
}
