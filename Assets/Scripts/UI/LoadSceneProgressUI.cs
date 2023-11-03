using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadSceneProgressUI : ISingleton<LoadSceneProgressUI>
{
    /// <summary>
    /// 进度条
    /// </summary>
    private Slider progressSlider;
    /// <summary>
    /// 进度值
    /// </summary>
    private Text progressText;

    private CanvasGroup canvasGroup;

    /// <summary>
    /// 透明度改变时间
    /// </summary>
    public float AlphaTime = 1f;

    private float timeCount;
    private bool isOver = false;

    /// <summary>
    /// 进度条改变
    /// </summary>
    public  UnityAction<float> OnProgressChanged;
    /// <summary>
    /// 结束
    /// </summary>
    public  UnityAction OnProgressDone;
    /// <summary>
    /// 开始
    /// </summary>
    public  UnityAction OnProgressStart;

    protected override void Awake()
    {
        base.Awake();

        progressSlider = transform.Find("Slider").GetComponent<Slider>();
        progressText = progressSlider.transform.Find("Progress").GetComponent<Text>();
        canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        timeCount = AlphaTime;

        OnProgressChanged += LoadSceneProgressUI_OnProgressChanged;
        OnProgressDone += LoadSceneProgressUI_OnProgressDone;
        OnProgressStart += LoadSceneProgressUI_OnProgressStart;
    }

    /// <summary>
    /// 进度条结束
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void LoadSceneProgressUI_OnProgressDone()
    {
        isOver = true;
        timeCount = AlphaTime;
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// 进度条结束
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void LoadSceneProgressUI_OnProgressStart()
    {
        isOver = false;
        timeCount = AlphaTime;
        canvasGroup.alpha = 1;
    }

    private void Update()
    {
        if(timeCount > 0 && isOver)
        {
            canvasGroup.alpha -= Time.deltaTime;
            timeCount -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 进度条改变
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
