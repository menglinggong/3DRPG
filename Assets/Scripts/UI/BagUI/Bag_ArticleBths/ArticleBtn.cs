using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class ArticleBtnEvent : UnityEvent
{

}

/// <summary>
/// ��Ʒ���ܰ�ť
/// </summary>
public class ArticleBtn : MonoBehaviour
{
    /// <summary>
    /// ����Ĺ�ѡ��
    /// </summary>
    private Toggle toggle;

    [Header("�Ƿ�һֱ��ʾ")]
    [SerializeField]
    private bool isAlwaysShow = false;

    [Header("֧�ֵ���Ʒ����")]
    [SerializeField]
    private List<int> supportKeys = new List<int>();

    private bool isOn;

    private UnityAction<ArticleBtn, bool> onValueChanged;

    public UnityAction<ArticleBtn, bool> OnValueChanged
    {
        get { return onValueChanged; }
        set { onValueChanged = value; }
    }

    /// <summary>
    /// ȷ��ѡ���¼�
    /// </summary>
    public ArticleBtnEvent onBtnClick = new ArticleBtnEvent();

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        isOn = toggle.isOn;
        toggle.onValueChanged.AddListener((ison) =>
        {
            if (ison == this.isOn) return;

            this.isOn = ison;
            onValueChanged?.Invoke(this, isOn);
        });
    }

    #region �ⲿ����

    /// <summary>
    /// �ж��Ƿ���֧����Ʒ�б���
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool CheckSupport(int key)
    {
        if(isAlwaysShow)
            return true;

        return supportKeys.Contains(key);
    }

    /// <summary>
    /// ��ʾ��ť
    /// </summary>
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// ���ذ�ť
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// ����ѡ��
    /// </summary>
    /// <param name="isOn"></param>
    public void SetSelected(bool isOn)
    {
        toggle.isOn = isOn;
    }

    #endregion
}
