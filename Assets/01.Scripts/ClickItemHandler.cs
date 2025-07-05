using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public enum ItemType { Slippers, Door, Fusebox, Radio, Velve, Ipad };

//TODO: click & zoom이 좋을듯!
public class ClickItemHandler : MonoBehaviour
{
    ActionBasedController actionController;

    public ItemType type;

    public GameObject controller;

    private XRRayInteractor rayInteractor;
    private GameObject hoveredObject;

    void Start()
    {
        rayInteractor = GetComponentInChildren<XRRayInteractor>();

        if (rayInteractor != null)
        {
            rayInteractor.hoverEntered.AddListener(OnHoverEnter);
            rayInteractor.hoverExited.AddListener(OnHoverExit);
        }
    }

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

    void OnDestroy()
    {
        if (rayInteractor != null)
        {
            rayInteractor.hoverEntered.RemoveListener(OnHoverEnter);
            rayInteractor.hoverExited.RemoveListener(OnHoverExit);
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        hoveredObject = args.interactableObject.transform.gameObject;
        Debug.Log($"[ClickItemHandler] Hover Enter: {hoveredObject.name}");
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        if (hoveredObject == args.interactableObject.transform.gameObject)
        {
            Debug.Log($"[ClickItemHandler] Hover Exit: {hoveredObject.name}");
            hoveredObject = null;
        }
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
        if (hoveredObject == null) return;
        string targetName = hoveredObject.name;

        if (targetName.StartsWith("@"))
            targetName = targetName.Substring(1);
        if (!Enum.TryParse<ItemType>(targetName, ignoreCase: true, out ItemType hitItemType))
        {
            return;
        }

        type = hitItemType;
        
        var zoomManager = FindObjectOfType<ZoomManager>();
        if (zoomManager == null || zoomManager.IsZoomedIn) return;

        switch (hitItemType)
        {
            case ItemType.Door:
                hoveredObject.GetComponent<HingedDoor>()?.Toggle();
                MissionManager.Instance.CompleteMission(MissionState.door);
                break;
            case ItemType.Slippers:
            case ItemType.Fusebox:
            case ItemType.Radio:
            case ItemType.Velve:
            case ItemType.Ipad:
                zoomManager.EnterZoomMode(hoveredObject);
                break;
        }
    }
}
