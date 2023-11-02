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
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void OnContinueGameBtnClick()
    {

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
}
