using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        Init();
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

}

