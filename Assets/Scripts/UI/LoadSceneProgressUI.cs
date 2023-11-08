using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.ObjectChangeEventStream;

public class LoadSceneProgressUI : MonoBehaviour
{
    /// <summary>
    /// ������
    /// </summary>
    private Slider progressSlider;
    /// <summary>
    /// ����ֵ
    /// </summary>
    private Text progressText;
    /// <summary>
    /// CanvasGroup
    /// </summary>
    private CanvasGroup canvasGroup;

    /// <summary>
    /// ͸���ȸı�ʱ��
    /// </summary>
    public float AlphaTime = 1f;

    private float timeCount;
    private bool isOver = false;

    /// <summary>
    /// �������ı�
    /// </summary>
    public  UnityAction<float> OnProgressChanged;
    /// <summary>
    /// ����
    /// </summary>
    public  UnityAction OnProgressDone;
    /// <summary>
    /// ��ʼ
    /// </summary>
    public  UnityAction OnProgressStart;

    private void Awake()
    {
        progressSlider = transform.Find("Slider").GetComponent<Slider>();
        progressText = progressSlider.transform.Find("Progress").GetComponent<Text>();
        canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        timeCount = AlphaTime;

        OnProgressChanged += LoadSceneProgressUI_OnProgressChanged;
        OnProgressDone += LoadSceneProgressUI_OnProgressDone;
        OnProgressStart += LoadSceneProgressUI_OnProgressStart;
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void LoadSceneProgressUI_OnProgressDone()
    {
        isOver = true;
        timeCount = AlphaTime;
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// ��������ʼ
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void LoadSceneProgressUI_OnProgressStart()
    {
        isOver = false;
        timeCount = AlphaTime;
        canvasGroup.alpha = 1;
        progressSlider.value = 0;
        progressText.text = "0%";
    }

    private void Update()
    {
        if(timeCount > 0 && isOver)
        {
            canvasGroup.alpha -= (Time.deltaTime / timeCount);
            timeCount -= Time.deltaTime;
        }
    }

    /// <summary>
    /// �������ı�
    /// </summary>
    /// <param name="arg0"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void LoadSceneProgressUI_OnProgressChanged(float progress)
    {
        progressSlider.value = progress;
        StringBuilder builder = new StringBuilder((progress * 100f).ToString());
        builder.Append("%");
        progressText.text = builder.ToString();
    }
}
