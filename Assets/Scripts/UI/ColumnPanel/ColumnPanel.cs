using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��������
/// </summary>
public class ColumnPanel : MonoBehaviour
{
    /// <summary>
    /// ����toggle
    /// </summary>
    [SerializeField]
    private List<LabelTipToggle> labelTipToggles = new List<LabelTipToggle>();

    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField]
    private List<LabelDataBase> labelDatas = new List<LabelDataBase>();

    /// <summary>
    /// ��ǰѡ�е�toggle
    /// </summary>
    private int currentSelectedIndex = 0;

    /// <summary>
    /// ���ռ�������ķ�Ӧʱ��
    /// </summary>
    private float reactionTime = 2.5f;

    /// <summary>
    /// ��ʱ
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
    /// ��ҡ������
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

