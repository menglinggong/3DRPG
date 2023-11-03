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

        EnterScene01();
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void OnContinueGameBtnClick()
    {
        EnterScene01();
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

    /// <summary>
    /// �����ǿ�ʼ����Ϸ���Ǽ�����Ϸ�������һ��
    /// </summary>
    private void EnterScene01()
    {
        //����Level01����
        TransitionPoint transitionPoint = new TransitionPoint();
        transitionPoint.SceneName = "Level01";
        transitionPoint.Type_Transition = TransitionPoint.TransitionType.DiffenceScene;
        transitionPoint.Type_Destination = TransitionDestination.DestinationType.Scene01_A;

        ScenesManager.Instance.TransitionToDestination(transitionPoint);
    }
}
