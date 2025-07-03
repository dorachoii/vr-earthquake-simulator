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

    void Start()
    {
        mainCamera = Camera.main;
        xrRayInteractor = FindObjectOfType<XRRayInteractor>();

        foreach (var cam in zoomCameras)
            if (cam != null) cam.gameObject.SetActive(false);
    }

    public void EnterZoomMode(GameObject targetObject)
    {
        if (isZoomedIn) return;

        activeZoomCamera = FindZoomCameraForTarget(targetObject);
        if (activeZoomCamera == null) return;

        isZoomedIn = true;

        mainCamera?.gameObject.SetActive(false);
        activeZoomCamera.gameObject.SetActive(true);
        if (xrRayInteractor != null) xrRayInteractor.enabled = false;
        zoomInputUI?.SetActive(true);
    }

    public void ExitZoomMode()
    {
        if (!isZoomedIn) return;

        mainCamera?.gameObject.SetActive(true);
        if (activeZoomCamera != null) activeZoomCamera.gameObject.SetActive(false);
        if (xrRayInteractor != null) xrRayInteractor.enabled = true;
        zoomInputUI?.SetActive(false);

        activeZoomCamera = null;
        isZoomedIn = false;
    }

    private Camera FindZoomCameraForTarget(GameObject targetObject)
    {
        foreach (var cam in zoomCameras)
            if (cam != null && cam.name.Contains(targetObject.name))
                return cam;

        return null;
    }
}
