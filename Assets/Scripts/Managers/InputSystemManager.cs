using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

/// <summary>
/// 输入系统的管理器
/// </summary>
public class InputSystemManager : ISingleton<InputSystemManager>
{
    private InputController inputAction;

    [SerializeField]
    private EventSystem m_eventSystem;

    private GameObject lastSelectedObj = null;

    #region 生命周期函数

    protected override void Awake()
    {
        base.Awake();

        //SetMouseInvalid();

        inputAction = new InputController();
        inputAction.Enable();

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
        if (lastSelectedObj != null)
            SetMouseClickInvalid();

        KeyBoardInput();
        GamePadInput();
    }

    #endregion

    #region 内部方法

    /// <summary>
    /// 键盘输入
    /// </summary>
    private void KeyBoardInput()
    {
        if (Keyboard.current == null)
            return;

        //左方向键输入
        Vector2 leftStick = inputAction.Keyboard.LeftStick.ReadValue<Vector2>();
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnLeftStick, leftStick);

        //右方向键输入
        Vector2 rightStick = inputAction.Keyboard.RightStick.ReadValue<Vector2>();
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnRightStick, rightStick);
    }

    /// <summary>
    /// 手柄输入
    /// </summary>
    private void GamePadInput()
    {
        if (Gamepad.current == null)
            return;

        //左方向键输入
        Vector2 leftStick = inputAction.GamePad.LeftStick.ReadValue<Vector2>();
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnLeftStick, leftStick);

        //右方向键输入
        Vector2 rightStick = inputAction.GamePad.RightStick.ReadValue<Vector2>();
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnRightStick, rightStick);
    }

    /// <summary>
    /// 设置鼠标不可用
    /// </summary>
    private void SetMouseInvalid()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// 设置鼠标点击后，依旧选中最后的界面元素
    /// </summary>
    private void SetMouseClickInvalid()
    {
        if(m_eventSystem.currentSelectedGameObject != null)
            lastSelectedObj = m_eventSystem.currentSelectedGameObject;
        else
            m_eventSystem.SetSelectedGameObject(lastSelectedObj);
    }

    #endregion

    #region 按键功能，发送消息

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

    #endregion

    #region 外部方法

    /// <summary>
    /// 设置界面元素当前选中的元素
    /// </summary>
    /// <param name="obj"></param>
    public void SetCurrentSelectedObj(GameObject obj)
    {
        m_eventSystem.SetSelectedGameObject(obj);
    }

    /// <summary>
    /// 获取界面中当前选中的元素
    /// </summary>
    /// <returns></returns>
    public GameObject GetCurrentSelectedObj()
    {
        return m_eventSystem.currentSelectedGameObject;
    }

    /// <summary>
    /// 设置界面最后选中的元素
    /// </summary>
    /// <param name="obj"></param>
    public void SetLastSelectedObj(GameObject obj)
    {
        lastSelectedObj = obj;
    }

    #endregion
}
