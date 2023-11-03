using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 暂停界面
/// </summary>
public class PauseUI : MonoBehaviour
{
    /// <summary>
    /// 继续游戏按钮
    /// </summary>
    private Button continueGameBtn;

    /// <summary>
    /// 返回主界面按钮
    /// </summary>
    private Button returnMenuBtn;

    /// <summary>
    /// 退出按钮
    /// </summary>
    private Button quitBtn;

    /// <summary>
    /// 暂停的背景
    /// </summary>
    private GameObject bg;

    private void Awake()
    {
        bg = transform.Find("BG").gameObject;
        continueGameBtn = transform.Find("BG/ContinueGameBtn").GetComponent<Button>();
        returnMenuBtn = transform.Find("BG/ReturnMenuBtn").GetComponent<Button>();
        quitBtn = transform.Find("BG/QuitBtn").GetComponent<Button>();

        continueGameBtn.onClick.AddListener(OnContinueGameBtnClick);
        returnMenuBtn.onClick.AddListener(OnReturnMenuBtnClick);
        quitBtn.onClick.AddListener(OnQuitBtnClick);

        bg.SetActive(false);
    }

    /// <summary>
    /// 按下ESC打开暂停界面
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !SceneManager.GetActiveScene().name.Equals("Main"))
        {
            bg.SetActive(true);
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    private void OnContinueGameBtnClick()
    {
        bg.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    private void OnReturnMenuBtnClick()
    {
        bg.SetActive(false);
        ScenesManager.Instance.LoadScene("Main");
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    private void OnQuitBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
