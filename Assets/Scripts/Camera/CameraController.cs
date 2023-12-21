using Cinemachine;
using System;
using UnityEngine;

/// <summary>
/// 相机控制器
/// </summary>
public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook freeLookCamera;
    private float cameraYValue = 0;

    private void Awake()
    {
        freeLookCamera = this.GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        if(freeLookCamera.LookAt == null && GameManager.Instance.PlayerStats != null)
        {
            freeLookCamera.Follow = GameManager.Instance.PlayerStats.transform.Find("head");
            freeLookCamera.LookAt = GameManager.Instance.PlayerStats.transform.Find("head");
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(MessageConst.InputSystemConst.OnRightStick, OnRightStickInput);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(MessageConst.InputSystemConst.OnRightStick, OnRightStickInput);
    }

    /// <summary>
    /// 手柄右摇杆/键盘右方向键输入
    /// </summary>
    /// <param name="messageConst"></param>
    /// <param name="data"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnRightStickInput(string messageConst, object data)
    {
        Vector2 value = (Vector2)data;

        cameraYValue += value.y * Time.deltaTime;
        cameraYValue = Mathf.Clamp(cameraYValue, 0, 1);

        freeLookCamera.m_YAxis.Value = cameraYValue;
        freeLookCamera.m_XAxis.m_InputAxisValue = value.x;
        
        EventManager.Instance.Invoke(MessageConst.OnCameraMove, freeLookCamera.m_XAxis.Value);
    }
}
