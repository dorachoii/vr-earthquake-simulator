using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public enum ItemType {Slipper, Door, Fusebox, Radio};

//TODO: click & zoom이 좋을듯!
public class ClickItemHandler : MonoBehaviour
{
    ActionBasedController actionController;
    public GameObject controller;

    public ItemType type;

    void OnEnable()
    {
        actionController = controller.GetComponent<ActionBasedController>();

        var activeAction = actionController.activateAction.action;

        if (activeAction != null)
        {
            activeAction.performed += OnActivatePerformed;
            activeAction.Enable();
        }
    }


    void OnDisable()
    {
        var activateAction = actionController.activateAction.action;
        var UIPressAction = actionController.uiPressAction.action;

        if (activateAction != null)
            activateAction.performed -= OnActivatePerformed;

        if (UIPressAction != null)
            UIPressAction.performed -= OnUIPressPerformed;
    }

    private void OnUIPressPerformed(InputAction.CallbackContext context)
    {
        var zoomManager = FindObjectOfType<ZoomManager>();

        if (zoomManager != null)
        {
            zoomManager.ExitZoomMode();
            gameObject.GetComponent<Outline>().enabled = false;
            return;
        }
    }

    private void OnActivatePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("actionPerformed");
        var zoomManager = FindObjectOfType<ZoomManager>();

        if (zoomManager != null)
        {
            if (!zoomManager.IsZoomedIn)
            {
                zoomManager.EnterZoomMode(this.gameObject);
                return;
            }
        }
    }
}
