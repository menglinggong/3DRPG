using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

/// <summary>
/// ����ϵͳ�Ĺ�����
/// </summary>
public class InputSystemManager : ISingleton<InputSystemManager>
{
    private InputController inputAction;
    private PlayerController playerController;

    private CinemachineFreeLook freeLook;
    public float cameraYValue = 0;
    public float cameraXValue = 0;

    protected override void Awake()
    {
        base.Awake();

        inputAction = new InputController();
        inputAction.Enable();

        inputAction.Keyboard.Jump.performed += Jump;
    }

    private void Update()
    {
        //�ӽ��ƶ�
        if (freeLook == null)
            freeLook = GameObject.FindObjectOfType<CinemachineFreeLook>();

        if (freeLook == null)
            return;

        if (playerController == null && GameManager.Instance.PlayerStats != null)
            playerController = GameManager.Instance.PlayerStats.GetComponent<PlayerController>();

        if (playerController == null)
            return;

        NewInputSystem();
        GamePadInput();
    }

    public void Jump(InputAction.CallbackContext context)
    {
    }

    /// <summary>
    /// ������ϵͳ
    /// </summary>
    private void NewInputSystem()
    {
        //����ƶ�
        Vector2 cameraMent = inputAction.Keyboard.Camera.ReadValue<Vector2>();
        CameraMove(cameraMent);
        //����ƶ�
        Vector2 moveMent = inputAction.Keyboard.Move.ReadValue<Vector2>();
        PlayerMove(moveMent);
    }

    /// <summary>
    /// �ֱ�����
    /// </summary>
    private void GamePadInput()
    {
        //����ƶ�
        Vector2 cameraMent = inputAction.GamePad.Camera.ReadValue<Vector2>();
        CameraMove(cameraMent);
        //����ƶ�
        Vector2 moveMent = inputAction.GamePad.Move.ReadValue<Vector2>();
        PlayerMove(moveMent);
    }

    /// <summary>
    /// ����ӽ�ת��
    /// </summary>
    /// <param name="value"></param>
    private void CameraMove(Vector2 value)
    {
        cameraYValue += value.y * Time.deltaTime;
        cameraYValue = Mathf.Clamp(cameraYValue, 0, 1);

        freeLook.m_YAxis.Value = cameraYValue;
        freeLook.m_XAxis.m_InputAxisValue = value.x;
        cameraXValue = freeLook.m_XAxis.Value;
    }

    /// <summary>
    /// ����ƶ��������ӽǵı䶯�����ǰ������Ҳ��ı�
    /// </summary>
    /// <param name="value"></param>
    private void PlayerMove(Vector2 value)
    {
        Vector3 moveValue = new Vector3(value.x, 0, value.y);
        Quaternion pianyi = Quaternion.Euler(0, cameraXValue, 0);

        moveValue = pianyi * moveValue;
        playerController.Move(moveValue);
    }
}
