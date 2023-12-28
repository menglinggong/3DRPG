using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������Ʒѡ��ʱ����ʾ��Ʒ��Ϣ
/// ѡ�в���װ������Ʒ
/// </summary>
public class Bag_ArticleInfo : MonoBehaviour
{
    [Header("��Ʒ�����ı�")]
    [SerializeField]
    private Text articleName;

    [Header("��Ʒ������Ϣ�ı�")]
    [SerializeField]
    private Text articleDescrip;

    [Header("��Ʒ����ͼƬ")]
    [SerializeField]
    private Image typeIcon;

    [Header("��Ʒֵ")]
    [SerializeField]
    private Text value;

    private void Start()
    {
        EventManager.Instance.AddListener(MessageConst.ArticleConst.OnArticleUISelected, OnArticleUISelected);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(MessageConst.ArticleConst.OnArticleUISelected, OnArticleUISelected);
    }

    /// <summary>
    /// ��Ʒѡ��
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnArticleUISelected(string arg0, object arg1)
    {
        ArticleInfoBase info = arg1 as ArticleInfoBase;

        SetShowName(info.Name);
        SetShowDescrip(info.Descrip);
    }


    #region �ⲿ����

    /// <summary>
    /// ������ʾ����
    /// </summary>
    /// <param name="name"></param>
    public void SetShowName(string name)
    {
        articleName.text = name;
    }

    /// <summary>
    /// ������ʾ������
    /// </summary>
    /// <param name="descrip"></param>
    public void SetShowDescrip(string descrip)
    {
        articleDescrip.text = descrip;
    }

    /// <summary>
    /// ������Ʒ���ͼ�����
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    public void SetShowType(int id, string value)
    {

    }

    #endregion
}
