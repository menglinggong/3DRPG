using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

/// <summary>
/// ����ϵͳ�Ĺ�����
/// </summary>
public class InputSystemManager : ISingleton<InputSystemManager>
{
    private InputController inputAction;

    [SerializeField]
    private EventSystem m_eventSystem;

    private GameObject lastSelectedObj = null;

    #region �������ں���

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

    #region �ڲ�����

    /// <summary>
    /// ��������
    /// </summary>
    private void KeyBoardInput()
    {
        if (Keyboard.current == null)
            return;

        //���������
        Vector2 leftStick = inputAction.Keyboard.LeftStick.ReadValue<Vector2>();
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnLeftStick, leftStick);

        //�ҷ��������
        Vector2 rightStick = inputAction.Keyboard.RightStick.ReadValue<Vector2>();
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnRightStick, rightStick);
    }

    /// <summary>
    /// �ֱ�����
    /// </summary>
    private void GamePadInput()
    {
        if (Gamepad.current == null)
            return;

        //���������
        Vector2 leftStick = inputAction.GamePad.LeftStick.ReadValue<Vector2>();
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnLeftStick, leftStick);

        //�ҷ��������
        Vector2 rightStick = inputAction.GamePad.RightStick.ReadValue<Vector2>();
        EventManager.Instance.Invoke(MessageConst.InputSystemConst.OnRightStick, rightStick);
    }

    /// <summary>
    /// ������겻����
    /// </summary>
    private void SetMouseInvalid()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// ���������������ѡ�����Ľ���Ԫ��
    /// </summary>
    private void SetMouseClickInvalid()
    {
        if(m_eventSystem.currentSelectedGameObject != null)
            lastSelectedObj = m_eventSystem.currentSelectedGameObject;
        else
            m_eventSystem.SetSelectedGameObject(lastSelectedObj);
    }

    #endregion

    #region �������ܣ�������Ϣ

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

    #endregion

    #region �ⲿ����

    /// <summary>
    /// ���ý���Ԫ�ص�ǰѡ�е�Ԫ��
    /// </summary>
    /// <param name="obj"></param>
    public void SetCurrentSelectedObj(GameObject obj)
    {
        m_eventSystem.SetSelectedGameObject(obj);
    }

    /// <summary>
    /// ��ȡ�����е�ǰѡ�е�Ԫ��
    /// </summary>
    /// <returns></returns>
    public GameObject GetCurrentSelectedObj()
    {
        return m_eventSystem.currentSelectedGameObject;
    }

    /// <summary>
    /// ���ý������ѡ�е�Ԫ��
    /// </summary>
    /// <param name="obj"></param>
    public void SetLastSelectedObj(GameObject obj)
    {
        lastSelectedObj = obj;
    }

    #endregion
}
