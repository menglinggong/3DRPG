using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 分栏toggle
/// </summary>
public class LabelTipToggle : MonoBehaviour
{
    private Toggle tipToggle;

    private Text labelText;

    private Image[] images;

    /// <summary>
    /// 绑定的分栏数据界面
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

    #region 外部接口

    /// <summary>
    /// 设置togglegroup
    /// </summary>
    /// <param name="toggleGroup"></param>
    public void SetToggleGroup(ToggleGroup toggleGroup)
    {
        tipToggle.group = toggleGroup;
    }

    /// <summary>
    /// 设置显示文本
    /// </summary>
    /// <param name="showName"></param>
    public void SetShowName(string showName)
    {
        labelText.text = showName;
    }

    /// <summary>
    /// 设置显示图片
    /// </summary>
    /// <param name="sprite"></param>
    public void SetShowSprite(Sprite sprite)
    {
        images[0].sprite = sprite;
    }

    /// <summary>
    /// 设置选中时的图片
    /// </summary>
    /// <param name="sprite"></param>
    public void SetSelectSprite(Sprite sprite)
    {
        images[1].sprite = sprite;
    }

    /// <summary>
    /// 设置绑定的分栏数据界面
    /// </summary>
    /// <param name="data"></param>
    public void SetBindingData(LabelDataBase data)
    {
        bindngData = data;
    }

    /// <summary>
    /// 设置toggle选中
    /// </summary>
    public void SetToggleSelected()
    {
        tipToggle.isOn = true;
    }

    #endregion
}
