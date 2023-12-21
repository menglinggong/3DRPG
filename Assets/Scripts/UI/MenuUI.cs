using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

/// <summary>
/// 游戏主界面
/// </summary>
public class MenuUI : UIBase
{
    /// <summary>
    /// 新游戏
    /// </summary>
    private Button newGameBtn;
    /// <summary>
    /// 继续游戏
    /// </summary>
    private Button continueGameBtn;
    /// <summary>
    /// 退出游戏
    /// </summary>
    private Button quitBtn;

    /// <summary>
    /// timeline
    /// </summary>
    public PlayableDirector Director;

    private void Awake()
    {
        newGameBtn = this.transform.Find("NewGameBtn").GetComponent<Button>();
        continueGameBtn = this.transform.Find("ContinueGameBtn").GetComponent<Button>();
        quitBtn = this.transform.Find("QuitBtn").GetComponent<Button>();

        newGameBtn.onClick.AddListener(OnNewGameBtnClick);
        continueGameBtn.onClick.AddListener(OnContinueGameBtnClick);
        quitBtn.onClick.AddListener(OnQuitBtnClick);

        base.DefaultSelectedObj = newGameBtn.gameObject;
        base.OnShow();
    }

    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void OnNewGameBtnClick()
    {
        SaveDataManager.Instance.DeleteAllDatas();
        PlayTimeLine();

        //timeline播放结束跳转到Scene01场景
        Director.stopped += (dir) =>
        {
            EnterScene01();
        };
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void OnContinueGameBtnClick()
    {
        EnterScene01();
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void OnQuitBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// 无论是开始新游戏还是继续游戏都进入第一关
    /// </summary>
    private void EnterScene01()
    {
        //进入Level01场景
        TransitionPoint transitionPoint = new TransitionPoint();
        transitionPoint.SceneName = "Level01";
        transitionPoint.Type_Transition = TransitionPoint.TransitionType.DiffenceScene;
        transitionPoint.Type_Destination = TransitionDestination.DestinationType.Scene01_A;

        ScenesManager.Instance.TransitionToDestination(transitionPoint);
    }

    /// <summary>
    /// 播放timeline动画
    /// </summary>
    private void PlayTimeLine()
    {
        Director.Play();
    }

}
