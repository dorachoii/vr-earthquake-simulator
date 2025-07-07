using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashlightGrabHandler : MonoBehaviour
{
    [SerializeField] private Vector3 flashlightDownDirection = Vector3.down;
    
    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;
    public Transform controllerTransform;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        
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
        MissionManager.Instance.CompleteMission(MissionState.flashlight);
        isGrabbed = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        controllerTransform = null;
    }

    void Update()
    {
        if (isGrabbed && controllerTransform != null)
        {
            AdjustFlashlightDirection();
        }
    }

    private void AdjustFlashlightDirection()
    {
        Vector3 targetDirection = controllerTransform.forward;
        transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.back);
    }
} 