using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public enum ItemType { Slippers, Door, Fusebox, Radio, Velve, Ipad, flashlight };

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
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        if (hoveredObject == args.interactableObject.transform.gameObject)
        {
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
                var hingedDoor = hoveredObject.GetComponent<HingedDoor>();
                if (hingedDoor != null)
                {
                    if (hoveredObject.CompareTag("MissionDoor"))
                    {
                        var door = hingedDoor;
                        door.onOpend += () => MissionManager.Instance.CompleteMission(MissionState.door);
                        door.onClosed += () => MissionManager.Instance.RevertMission(MissionState.door);
                    }
                    hingedDoor.Toggle();
                }
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
