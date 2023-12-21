using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 分栏界面
/// </summary>
public class ColumnPanel : MonoBehaviour
{
    /// <summary>
    /// 分栏toggle
    /// </summary>
    [SerializeField]
    private List<LabelTipToggle> labelTipToggles = new List<LabelTipToggle>();

    /// <summary>
    /// 分栏界面
    /// </summary>
    [SerializeField]
    private List<LabelDataBase> labelDatas = new List<LabelDataBase>();

    /// <summary>
    /// 当前选中的toggle
    /// </summary>
    private int currentSelectedIndex = 0;

    /// <summary>
    /// 接收键盘输入的反应时间
    /// </summary>
    private float reactionTime = 2.5f;

    /// <summary>
    /// 计时
    /// </summary>
    private float timeCount = 0;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnRightStick, OnRightStickInput);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnRightStick, OnRightStickInput);
    }

    private void Update()
    {
        if (timeCount > 0)
        {
            timeCount -= Time.fixedUnscaledDeltaTime;
        }
    }

    private void Init()
    {
        for (int i = 0; i < labelTipToggles.Count; i++)
        {
            labelTipToggles[i].SetBindingData(labelDatas[i]);
            labelTipToggles[i].OnValueChanged += OnTipToggleValueChanged;
        }
    }

    private void OnTipToggleValueChanged(LabelTipToggle tipToggle, bool isOn)
    {
        tipToggle.BindngData.ShowHide(isOn);
    }

    /// <summary>
    /// 右摇杆输入
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnRightStickInput(string messageConst, object data)
    {
        Vector2 value = (Vector2)data;

        if (value.x == 0 || timeCount > 0)
            return;

        timeCount = reactionTime;

        if (value.x > 0)
            currentSelectedIndex++;
        else if(value.x < 0)
            currentSelectedIndex--;

        currentSelectedIndex = Mathf.Clamp(currentSelectedIndex, 0, labelTipToggles.Count - 1);

        labelTipToggles[currentSelectedIndex].SetToggleSelected();
    }
}

