using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏主界面
/// </summary>
public class MenuUI : MonoBehaviour
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

    private void Awake()
    {
        newGameBtn = this.transform.Find("NewGameBtn").GetComponent<Button>();
        continueGameBtn = this.transform.Find("ContinueGameBtn").GetComponent<Button>();
        quitBtn = this.transform.Find("QuitBtn").GetComponent<Button>();

        newGameBtn.onClick.AddListener(OnNewGameBtnClick);
        continueGameBtn.onClick.AddListener(OnContinueGameBtnClick);
        quitBtn.onClick.AddListener(OnQuitBtnClick);
    }

    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void OnNewGameBtnClick()
    {
        SaveDataManager.Instance.DeleteAllDatas();

        EnterScene01();
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
}
