using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ZoomedObjectExecutor : MonoBehaviour
{
    private ActionBasedController controller;
    private ZoomManager zoomManager;

    private void Start()
    {
        controller = GetComponent<ActionBasedController>();
        zoomManager = FindObjectOfType<ZoomManager>();

        var activate = controller?.activateAction.action;
        var uiPress = controller?.uiPressAction.action;

        if (activate != null)
            activate.performed += OnActivate;

        if (uiPress != null)
            uiPress.performed += OnUIPress;
    }

    private void OnDestroy()
    {
        var activate = controller?.activateAction.action;
        var uiPress = controller?.uiPressAction.action;

        if (activate != null)
            activate.performed -= OnActivate;

        if (uiPress != null)
            uiPress.performed -= OnUIPress;
    }

    private void OnActivate(InputAction.CallbackContext ctx)
    {
        if (zoomManager == null)
            return;

        GameObject target = zoomManager.CurrentTarget;
        if (target == null)
            return;

        // 줌 상태면 동작 처리
        var handler = target.GetComponent<ClickItemHandler>();
        if (handler == null)
            return;

        switch (handler.type)
        {
            case ItemType.Slipper:
                target.SetActive(false);
                MissionManager.Instance.CompleteCurrentMission();
                break;

            case ItemType.Door:
            case ItemType.Fusebox:
                var door = target.GetComponent<HingedDoor>();
                if (door != null)
                    door.Toggle();
                break;
        }
    }

    private void OnUIPress(InputAction.CallbackContext ctx)
    {
        if (zoomManager != null && zoomManager.IsZoomedIn)
        {
            zoomManager.ExitZoomMode();
        }
    }
}
