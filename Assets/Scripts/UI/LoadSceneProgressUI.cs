using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadSceneProgressUI : MonoBehaviour
{
    /// <summary>
    /// 进度条
    /// </summary>
    private Slider progressSlider;
    /// <summary>
    /// 进度值
    /// </summary>
    private Text progressText;
    /// <summary>
    /// CanvasGroup
    /// </summary>
    private CanvasGroup canvasGroup;

    /// <summary>
    /// 透明度改变时间
    /// </summary>
    public float AlphaTime = 1f;
    /// <summary>
    /// 计时
    /// </summary>
    private float timeCount;

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
    /// 进度条结束
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void LoadSceneProgressUI_OnProgressDone()
    {
        timeCount = AlphaTime;
        canvasGroup.alpha = 1;
        StartCoroutine(SetAlpha(true));
    }

    /// <summary>
    /// 进度条开始
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void LoadSceneProgressUI_OnProgressStart()
    {
        timeCount = AlphaTime;
        canvasGroup.alpha = 1;
        progressSlider.value = 0;
        progressText.text = "0%";
        //StartCoroutine(SetAlpha(false));
    }

    /// <summary>
    /// 设置界面透明度
    /// </summary>
    /// <param name="isMinus"></param>
    /// <returns></returns>
    IEnumerator SetAlpha(bool isMinus = false)
    {
        while(timeCount > 0)
        {
            if(isMinus)
                canvasGroup.alpha -= (Time.deltaTime / timeCount);
            else
                canvasGroup.alpha += (Time.deltaTime / timeCount);
            timeCount -= Time.deltaTime;
            yield return null;
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
