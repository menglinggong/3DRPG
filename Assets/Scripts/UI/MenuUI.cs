using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Ϸ������
/// </summary>
public class MenuUI : MonoBehaviour
{
    /// <summary>
    /// ����Ϸ
    /// </summary>
    private Button newGameBtn;
    /// <summary>
    /// ������Ϸ
    /// </summary>
    private Button continueGameBtn;
    /// <summary>
    /// �˳���Ϸ
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
    /// ��ʼ����Ϸ
    /// </summary>
    public void OnNewGameBtnClick()
    {
        SaveDataManager.Instance.DeleteAllDatas();
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void OnContinueGameBtnClick()
    {

    }

    /// <summary>
    /// �˳���Ϸ
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
