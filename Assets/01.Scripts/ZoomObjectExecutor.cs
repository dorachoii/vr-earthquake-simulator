using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ZoomedObjectExecutor : MonoBehaviour
{
    private ActionBasedController controller;
    private ZoomManager zoomManager;

    private bool isHolding = false;
    private Quaternion initialHandRotation;

    private void Start()
    {
        controller = GetComponent<ActionBasedController>();
        zoomManager = FindObjectOfType<ZoomManager>();

        var activate = controller?.activateAction.action;
        var uiPress = controller?.uiPressAction.action;
        var select = controller?.selectAction.action;

        if (activate != null) activate.performed += OnActivate;
        if (uiPress != null) uiPress.performed += OnUIPress;
        if (select != null)
        {
            select.started += ctx => StartHold();
            select.canceled += ctx => StopHold();
        }
    }

    private void OnDestroy()
    {
        var activate = controller?.activateAction.action;
        var uiPress = controller?.uiPressAction.action;

        if (activate != null) activate.performed -= OnActivate;
        if (uiPress != null) uiPress.performed -= OnUIPress;
    }

    private void OnActivate(InputAction.CallbackContext ctx)
    {
        if (zoomManager == null)
            return;

        GameObject target = zoomManager.CurrentTarget;
        if (target == null)
            return;

        // 줌 상태면 동작 처리
        var handler = FindAnyObjectByType<ClickItemHandler>();
        if (handler == null)
            return;

        switch (handler.type)
        {
            case ItemType.Slippers:
                target.SetActive(false);
                MissionManager.Instance.CompleteCurrentMission();
                break;
            case ItemType.Fusebox:
                target.gameObject.GetComponentInChildren<HingedDoor>().Toggle();
                MissionManager.Instance.CompleteCurrentMission();
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

    private void StartHold()
    {
        Debug.Log("Grab - started");
        if (zoomManager == null || !zoomManager.IsZoomedIn) return;

        GameObject target = zoomManager.CurrentTarget;
        if (target == null) return;

        var handler = FindAnyObjectByType<ClickItemHandler>();

        if (handler != null && handler.type == ItemType.Radio)
        {
            initialHandRotation = gameObject.transform.rotation;
            isHolding = true;
        }
    }

    private void StopHold()
    {
        Debug.Log("Grab - stopped");
        isHolding = false;
    }

    private void Update()
    {
        if (!isHolding || zoomManager == null || !zoomManager.IsZoomedIn) return;

        GameObject target = zoomManager.CurrentTarget;
        if (target == null) return;

        var radioDial = target.GetComponentInChildren<RadioDialHandler>();
        if (radioDial == null) return;

        Quaternion currentRotation = gameObject.transform.rotation;
        Quaternion deltaRotation = currentRotation * Quaternion.Inverse(initialHandRotation);

        float angleZ = Mathf.DeltaAngle(0, deltaRotation.eulerAngles.z);
        int direction = angleZ > 0 ? 1 : -1;

        radioDial.RotateDial(Mathf.Abs(angleZ), direction);
        initialHandRotation = currentRotation;
    }
}
