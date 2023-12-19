using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

/// <summary>
/// 输入系统的管理器
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
        //视角移动
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
    /// 键盘输入
    /// </summary>
    private void KeyBoardInput()
    {
        if (Keyboard.current == null)
            return;

        //相机移动
        Vector2 cameraMent = inputAction.Keyboard.Camera.ReadValue<Vector2>();
        CameraMove(cameraMent);
        //玩家移动
        Vector2 moveMent = inputAction.Keyboard.Move.ReadValue<Vector2>();
        PlayerMove(moveMent);
    }

    /// <summary>
    /// 手柄输入
    /// </summary>
    private void GamePadInput()
    {
        if (Gamepad.current == null)
            return;

        //相机移动
        Vector2 cameraMent = inputAction.GamePad.Camera.ReadValue<Vector2>();
        CameraMove(cameraMent);
        //玩家移动
        Vector2 moveMent = inputAction.GamePad.Move.ReadValue<Vector2>();
        PlayerMove(moveMent);
    }

    /// <summary>
    /// 相机视角转动
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
    /// 玩家移动，随着视角的变动，玩家前进方向也会改变
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
    /// A键点击，键盘上默认对应U键
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
    /// +键点击
    /// </summary>
    /// <param name="context"></param>
    private void OnPlusPerformed(InputAction.CallbackContext context)
    {
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnPlusPerformed, null);
    }
}
