using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashlightGrabHandler : MonoBehaviour
{
    [SerializeField] private Vector3 flashlightDownDirection = Vector3.down;
    
    private XRGrabInteractable grabInteractable;
    private Camera mainCamera;
    private bool isGrabbed = false;
    private bool hasAdjustedDirection = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        mainCamera = Camera.main;
        
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        hasAdjustedDirection = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        hasAdjustedDirection = false;
    }

    void Update()
    {
        if (isGrabbed && mainCamera != null && !hasAdjustedDirection)
        {
            AdjustFlashlightDirection();
            hasAdjustedDirection = true;
        }
    }

    private void AdjustFlashlightDirection()
    {
        // 방향 조정 - 손전등의 down 방향을 카메라의 forward 방향과 일치
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 currentFlashlightDown = transform.TransformDirection(flashlightDownDirection);
        Quaternion rotationAdjustment = Quaternion.FromToRotation(currentFlashlightDown, cameraForward);
        transform.rotation = rotationAdjustment * transform.rotation;
    }
} 