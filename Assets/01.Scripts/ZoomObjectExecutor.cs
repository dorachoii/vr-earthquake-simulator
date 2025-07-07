using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ZoomedObjectExecutor : MonoBehaviour
{
    private ActionBasedController controller;
    private ZoomManager zoomManager;

    private bool isHolding = false;
    private Quaternion initialHandRotation;
    private Vector3 originalTabletPosition;
    private Quaternion originalTabletRotation;
    private bool hasStoredOriginalTransform = false;

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
        
        enabled = false;
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
                MissionManager.Instance.CompleteMission(MissionState.slippers);
                break;
            case ItemType.Fusebox:
                var fuseboxDoor = target.gameObject.GetComponentInChildren<HingedDoor>();
                if (fuseboxDoor != null)
                {
                    fuseboxDoor.onOpend += () => MissionManager.Instance.CompleteMission(MissionState.fusebox);
                    fuseboxDoor.onClosed += () => MissionManager.Instance.RevertMission(MissionState.fusebox);
                    fuseboxDoor.Toggle();
                }
                break;
            case ItemType.Velve:
                var velveDoor = target.gameObject.GetComponentInChildren<HingedDoor>();
                if (velveDoor != null)
                {
                    velveDoor.onOpend += () => MissionManager.Instance.CompleteMission(MissionState.gasVelve);
                    velveDoor.onClosed += () => MissionManager.Instance.RevertMission(MissionState.gasVelve);
                    velveDoor.Toggle();
                }
                break;
            case ItemType.Ipad:
                if (!hasStoredOriginalTransform)
                {
                    originalTabletPosition = target.transform.position;
                    originalTabletRotation = target.transform.rotation;
                    hasStoredOriginalTransform = true;
                }
                StartCoroutine(SmoothMoveToCamera(target));
                MissionManager.Instance.CompleteMission(MissionState.tablet);
                break;
        }
    }

    private void OnUIPress(InputAction.CallbackContext ctx)
    {
        if (zoomManager != null && zoomManager.IsZoomedIn)
        {
            GameObject target = zoomManager.CurrentTarget;
            if (target != null && hasStoredOriginalTransform)
            {
                var handler = FindAnyObjectByType<ClickItemHandler>();
                if (handler != null && handler.type == ItemType.Ipad)
                {
                    StartCoroutine(RestoreTabletPosition(target));
                }
            }
            
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
            target.GetComponentInChildren<RadioDialHandler>().TurnOnRadio();
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

    private IEnumerator SmoothMoveToCamera(GameObject target)
    {
        if (zoomManager == null) yield break;
        
        Camera zoomCamera = null;
        foreach (var cam in zoomManager.zoomCameras)
        {
            if (cam != null && cam.gameObject.activeInHierarchy)
            {
                zoomCamera = cam;
                break;
            }
        }
        
        if (zoomCamera == null) yield break;

        Vector3 startPosition = target.transform.position;
        Quaternion startRotation = target.transform.rotation;
        Vector3 targetPosition = zoomCamera.transform.position + zoomCamera.transform.forward * 3f; // 줌 카메라 앞 2f 거리
        Quaternion targetRotation = Quaternion.LookRotation(- zoomCamera.transform.forward, Vector3.up); // 카메라를 향하는 회전
        float duration = 1.0f; 
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            target.transform.position = Vector3.Lerp(startPosition, targetPosition, smoothT);
            target.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, smoothT);
            
            yield return null;
        }

        target.transform.position = targetPosition;
        target.transform.rotation = targetRotation;
    }

    private IEnumerator RestoreTabletPosition(GameObject target)
    {
        Vector3 startPosition = target.transform.position;
        Quaternion startRotation = target.transform.rotation;
        float duration = 1.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            target.transform.position = Vector3.Lerp(startPosition, originalTabletPosition, smoothT);
            target.transform.rotation = Quaternion.Lerp(startRotation, originalTabletRotation, smoothT);
            
            yield return null;
        }

        target.transform.position = originalTabletPosition;
        target.transform.rotation = originalTabletRotation;
        
        hasStoredOriginalTransform = false;
    }
}
