using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ����-��Ʒ���ܰ�ť��װ����ж�£�ʳ�ã��ֳֵȣ�
/// </summary>
public class Bag_ArticleBtns : MonoBehaviour
{
    [Header("���а�ť")]
    [SerializeField]
    private List<ArticleBtn> articleBtns = new List<ArticleBtn>();

    /// <summary>
    /// ��ʾ�İ�ť
    /// </summary>
    private List<ArticleBtn> showBtns = new List<ArticleBtn>();

    /// <summary>
    /// ��ǰѡ�еİ�ť
    /// </summary>
    private ArticleBtn currentSelectedBtn;

    /// <summary>
    /// ��ǰѡ�еİ�ť���±�
    /// </summary>
    private int currentIndex = 0;

    /// <summary>
    /// ���ռ�������ķ�Ӧʱ��
    /// </summary>
    private float reactionTime = 1f;

    /// <summary>
    /// ��ʱ
    /// </summary>
    private float timeCount = 0;

    private UnityAction onPanelClose;

    public UnityAction OnPanelClose
    {
        get { return onPanelClose; }
        set { onPanelClose = value; }
    }

    #region �������ں���

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

    #region �������

    /// <summary>
    /// ȷ��
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnConfirm(string messageConst, object data)
    {
        //TODO���ݵ�ǰѡ�еİ�ť��ִ�ж�Ӧ����
        currentSelectedBtn.onBtnClick?.Invoke();
    }

    /// <summary>
    /// ȡ��
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnCancel(string messageConst, object data)
    {
        Hide();
    }

    /// <summary>
    /// �л�ѡ�а�ť
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

    #region �ڲ�����

    

    /// <summary>
    /// ��ʾĬ�ϵİ�ť
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
                //TODO�ж��Ƿ�Ϊ�������������ƣ������࣬������Ҫ��������Ƿ��Ѿ�װ�����е���

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
    /// ����������
    /// </summary>
    private void AddListener()
    {
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnAPerformed, OnConfirm);
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnBPerformed, OnCancel);
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnLeftStick, OnSwitch);
    }

    /// <summary>
    /// �Ƴ�����ļ���
    /// </summary>
    private void RemoveListener()
    {
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnAPerformed, OnConfirm);
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnBPerformed, OnCancel);
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnLeftStick, OnSwitch);
    }

    #endregion

    #region �ⲿ����

    /// <summary>
    /// �򿪽���
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);
        AddListener();
        ShowDefaultBtns(ArticleManager.Instance.CurrentArticle.ID);
    }

    /// <summary>
    /// �رս���
    /// </summary>
    public void Hide()
    {
        RemoveListener();
        onPanelClose?.Invoke();
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// װ��
    /// </summary>
    public void OnEquipBtnClick()
    {
        Debug.Log("װ��");
        ArticleManager.Instance.EquipArticle(ArticleManager.Instance.CurrentArticle);
    }

    /// <summary>
    /// ж��
    /// </summary>
    public void OnDisboardBtnClick()
    {
        Debug.Log("ж��");
        ArticleManager.Instance.DisboardArticle(ArticleManager.Instance.CurrentArticle);
    }

    /// <summary>
    /// ����
    /// </summary>
    public void OnDiscardBtnClick()
    {
        Debug.Log("����");
        ArticleManager.Instance.DiscardArticle(new List<ArticleInfoBase>() { ArticleManager.Instance.CurrentArticle });
    }

    /// <summary>
    /// ʳ��
    /// </summary>
    public void OnEatBtnClick()
    {
        //�޸����ݿ�����
        ArticleManager.Instance.UseArticle(ArticleManager.Instance.CurrentArticle);
        //ˢ����Ʒ����
        ArticleManager.Instance.CurrentItemFram.Refresh();
        //TODOʵ����Ʒ��Ч��
        Hide();
    }

    /// <summary>
    /// �ֳ�
    /// </summary>
    public void OnHandBtnClick()
    {
        Debug.Log("�ֳ�");
        ArticleManager.Instance.HoldArticle(new List<ArticleInfoBase>() { ArticleManager.Instance.CurrentArticle });
    }

    /// <summary>
    /// �鿴
    /// </summary>
    public void OnCheckBtnClick()
    {
        Debug.Log("�鿴");
        ArticleManager.Instance.CheckArticle(ArticleManager.Instance.CurrentArticle);
    }

    /// <summary>
    /// ȡ��
    /// </summary>
    public void OnCanelBtnClick()
    {
        Hide();
    }

    #endregion
}
