using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// ����toggle
/// </summary>
public class LabelTipToggle : MonoBehaviour
{
    private Toggle tipToggle;

    private Text labelText;

    private Image[] images;

    /// <summary>
    /// �󶨵ķ������ݽ���
    /// </summary>
    private LabelDataBase bindngData;

    private bool isOn;

    private UnityAction<LabelTipToggle, bool> onValueChanged;

    public UnityAction<LabelTipToggle, bool> OnValueChanged
    {
        get { return onValueChanged; }
        set { onValueChanged = value; }
    }

    public LabelDataBase BindngData
    {
        get { return bindngData; }
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        tipToggle = GetComponent<Toggle>();
        labelText = GetComponentInChildren<Text>();
        images = GetComponentsInChildren<Image>();
        isOn = tipToggle.isOn;
    }

    private void OnEnable()
    {
        if (isOn)
        {
            onValueChanged?.Invoke(this, isOn);
        }

        tipToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn == this.isOn) return;

            this.isOn = isOn;
            onValueChanged?.Invoke(this, isOn);
        });
    }

    private void OnDisable()
    {
        tipToggle.onValueChanged.RemoveAllListeners();
    }

    #region �ⲿ�ӿ�

    /// <summary>
    /// ����togglegroup
    /// </summary>
    /// <param name="toggleGroup"></param>
    public void SetToggleGroup(ToggleGroup toggleGroup)
    {
        tipToggle.group = toggleGroup;
    }

    /// <summary>
    /// ������ʾ�ı�
    /// </summary>
    /// <param name="showName"></param>
    public void SetShowName(string showName)
    {
        labelText.text = showName;
    }

    /// <summary>
    /// ������ʾͼƬ
    /// </summary>
    /// <param name="sprite"></param>
    public void SetShowSprite(Sprite sprite)
    {
        images[0].sprite = sprite;
    }

    /// <summary>
    /// ����ѡ��ʱ��ͼƬ
    /// </summary>
    /// <param name="sprite"></param>
    public void SetSelectSprite(Sprite sprite)
    {
        images[1].sprite = sprite;
    }

    /// <summary>
    /// ���ð󶨵ķ������ݽ���
    /// </summary>
    /// <param name="data"></param>
    public void SetBindingData(LabelDataBase data)
    {
        bindngData = data;
    }

    /// <summary>
    /// ����toggleѡ��
    /// </summary>
    public void SetToggleSelected()
    {
        tipToggle.isOn = true;
    }

    #endregion
}
