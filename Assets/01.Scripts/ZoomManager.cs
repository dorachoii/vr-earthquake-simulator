using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ZoomManager : MonoBehaviour
{
    private Camera mainCamera;
    private XRRayInteractor xrRayInteractor;

    public Camera[] zoomCameras;
    public GameObject zoomInputUI;

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
        isZoomedIn = true;

        mainCamera?.gameObject.SetActive(false);
        activeZoomCamera.gameObject.SetActive(true);
        xrRayInteractor.enabled = false;
        zoomInputUI?.SetActive(true);
    }

    public void ExitZoomMode()
    {
        if (!isZoomedIn) return;

        mainCamera?.gameObject.SetActive(true);
        activeZoomCamera?.gameObject.SetActive(false);
        xrRayInteractor.enabled = true;
        zoomInputUI?.SetActive(false);

        activeZoomCamera = null;
        currentTargetObject = null;
        isZoomedIn = false;
    }

    private Camera FindZoomCameraForTarget(GameObject targetObject)
{
    string targetName = targetObject.name;

    if (targetName.StartsWith("@"))
        targetName = targetName.Substring(1); 

    Debug.Log($"[ZoomManager] Searching for ZoomCam matching: {targetName}");

    foreach (var cam in zoomCameras)
    {
        if (cam != null && cam.name.Contains(targetName))
            return cam;
    }

    Debug.LogWarning($"[ZoomManager] No zoom camera found for: {targetName}");
    return null;
}

}
