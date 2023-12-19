using Cinemachine;
using System;
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
    private float cameraYValue = 0;
    private float cameraXValue = 0;

    protected override void Awake()
    {
        base.Awake();

        inputAction = new InputController();
        inputAction.Enable();

        //inputAction.Keyboard.Jump.performed += Jump;
        if(Keyboard.current != null)
        {
            inputAction.Keyboard.A.performed += OnAPerformed;
            inputAction.Keyboard.Plus.performed += OnPlusPerformed;
        }

        if(Gamepad.current != null)
        {
            inputAction.GamePad.A.performed += OnAPerformed;
            inputAction.GamePad.Plus.performed += OnPlusPerformed;
        }
    }

    private void OnDisable()
    {
        inputAction.Disable();
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

        KeyBoardInput();
        GamePadInput();
    }

    public void Jump(InputAction.CallbackContext context)
    {
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void KeyBoardInput()
    {
        if (Keyboard.current == null)
            return;

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
        if (Gamepad.current == null)
            return;

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

    /// <summary>
    /// A�������������Ĭ�϶�ӦU��
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnAPerformed(InputAction.CallbackContext context)
    {
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnAPerformed, null);
    }

    private void OnBPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnXPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnYPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnLPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnZLPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnRerformed(InputAction.CallbackContext context)
    {

    }

    private void OnZRPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnUpPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnDownPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnLeftPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnRightPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnMinusPerformed(InputAction.CallbackContext context)
    {

    }

    /// <summary>
    /// +�����
    /// </summary>
    /// <param name="context"></param>
    private void OnPlusPerformed(InputAction.CallbackContext context)
    {
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnPlusPerformed, null);
    }
}
