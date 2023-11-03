using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ��ͣ����
/// </summary>
public class PauseUI : MonoBehaviour
{
    /// <summary>
    /// ������Ϸ��ť
    /// </summary>
    private Button continueGameBtn;

    /// <summary>
    /// ���������水ť
    /// </summary>
    private Button returnMenuBtn;

    /// <summary>
    /// �˳���ť
    /// </summary>
    private Button quitBtn;

    /// <summary>
    /// ��ͣ�ı���
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
    /// ����ESC����ͣ����
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
    /// ������Ϸ
    /// </summary>
    private void OnContinueGameBtnClick()
    {
        bg.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// �������˵�
    /// </summary>
    private void OnReturnMenuBtnClick()
    {
        bg.SetActive(false);
        ScenesManager.Instance.LoadScene("Main");
    }

    /// <summary>
    /// �˳���Ϸ
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
