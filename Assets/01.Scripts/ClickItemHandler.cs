using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class ClickItemHandler : MonoBehaviour
{
    public enum ClickItemType { splippers, fusebox, door }
    public static event Action<ClickItemType> OnClickItemCompleted;

    public ClickItemType type;

    ActionBasedController controller;

    void OnEnable()
    {
        if (controller == null)
            controller = GameObject.Find("Left Controller").GetComponent<ActionBasedController>();

        var action = controller.activateAction.action;
        if (action != null)
        {
            action.performed += OnActivatePerformed;
            action.Enable();
        }
    }

    void OnDisable()
    {
        var action = controller.activateAction.action;
        if (action != null)
        {
            action.performed -= OnActivatePerformed;
        }
    }

    private void OnActivatePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Activate 버튼 눌림");

        switch (type)
        {
            case ClickItemType.splippers:
                HandleSlipperClick();
                break;
            case ClickItemType.fusebox:
            case ClickItemType.door:
                HandleHingedClick();
                break;
        }
    }

    void HandleSlipperClick()
    {
        OnClickItemCompleted?.Invoke(type);
        gameObject.SetActive(false);
        Debug.Log("슬리퍼 클릭됨!");
    }

    void HandleHingedClick()
    {
        OnClickItemCompleted?.Invoke(type);
        gameObject.GetComponent<HingedDoor>().Toggle();
    }
}
