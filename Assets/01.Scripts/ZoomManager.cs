using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ZoomManager : MonoBehaviour
{
    private Camera mainCamera;
    private XRRayInteractor xrRayInteractor;

    public Camera[] zoomCameras;

    private bool isZoomedIn = false;
    private Camera activeZoomCamera = null;
    public bool IsZoomedIn => isZoomedIn;

    void Start()
    {
        mainCamera = Camera.main;
        xrRayInteractor = FindObjectOfType<XRRayInteractor>();

        foreach (var cam in zoomCameras)
            if (cam != null) cam.gameObject.SetActive(false);
    }

    private GameObject currentTargetObject;
    public GameObject CurrentTarget => currentTargetObject;

    public void EnterZoomMode(GameObject targetObject)
    {
        if (isZoomedIn) return;

        activeZoomCamera = FindZoomCameraForTarget(targetObject);
        if (activeZoomCamera == null) return;

        currentTargetObject = targetObject;
        Debug.Log($"currentTarget: {currentTargetObject.name}");
        isZoomedIn = true;

        activeZoomCamera.gameObject.SetActive(true);
        xrRayInteractor.enabled = false;
        
        StartCoroutine(EnableZoomExecutorAfterDelay(1f));
    }

    private IEnumerator EnableZoomExecutorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        var zoomExecutor = FindObjectOfType<ZoomedObjectExecutor>();
        if (zoomExecutor != null)
        {
            zoomExecutor.enabled = true;
        }
    }


    public void ExitZoomMode()
    {
        if (!isZoomedIn) return;

        activeZoomCamera?.gameObject.SetActive(false);
        xrRayInteractor.enabled = true;

        activeZoomCamera = null;
        currentTargetObject = null;
        isZoomedIn = false;

        var zoomExecutor = FindObjectOfType<ZoomedObjectExecutor>();
        zoomExecutor.enabled = false;
    }

    private Camera FindZoomCameraForTarget(GameObject targetObject)
    {
        string targetName = targetObject.name;

        if (targetName.StartsWith("@"))
            targetName = targetName.Substring(1);

        targetName.ToLower();

        foreach (var cam in zoomCameras)
        {
            if (cam != null && cam.name.Contains(targetName))
                return cam;
        }
        return null;
    }

}
