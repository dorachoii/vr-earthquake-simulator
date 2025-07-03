using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class ClickItemHandler : MonoBehaviour
{
    ActionBasedController actionController;
    public GameObject controller;

    void OnEnable()
    {
        actionController = controller.GetComponent<ActionBasedController>();


        var action = actionController.activateAction.action;
        if (action != null)
        {
            action.performed += OnActivatePerformed;
            action.Enable();
        }
    }


    void OnDisable()
    {
        var activateAction = actionController.activateAction.action;
        if (activateAction != null)
            activateAction.performed -= OnActivatePerformed;
    }


    private void OnActivatePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("actionPerformed");
        var zoomManager = FindObjectOfType<ZoomManager>();

        if (zoomManager != null)
        {
            zoomManager.EnterZoomMode(this.gameObject);
            return;
        }
    }


    // void HandleSlipperClick()
    // {
    //     OnClickItemCompleted?.Invoke(type);
    //     gameObject.SetActive(false);
    //     Debug.Log("슬리퍼 클릭됨!");
    // }

    // void HandleHingedClick()
    // {
    //     OnClickItemCompleted?.Invoke(type);
    //     gameObject.GetComponent<HingedDoor>().Toggle();
    // }
}
