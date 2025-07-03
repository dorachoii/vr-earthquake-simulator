using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabHandler : MonoBehaviour
{

    private GameObject grabbingHand;         // 잡은 손
    private Quaternion initialHandRotation;        // Grab 시작 시 손의 회전값
    private Quaternion initialDialRotation;        // Grab 시작 시 다이얼의 회전값

    RadioDialHandler radioDialHandler;

    private bool isGrabbing = false;

    private void Awake()
{
    radioDialHandler = GetComponent<RadioDialHandler>();
}


    private void OnEnable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnGrab);
            interactable.selectExited.AddListener(OnRelease);
        }
    }

    private void OnDisable()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnGrab);
            interactable.selectExited.RemoveListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("grab- start");
        isGrabbing = true;
        grabbingHand = GameObject.Find("Right Controller");

        initialHandRotation = grabbingHand.transform.rotation;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("grab- end");
        isGrabbing = false;
        grabbingHand = null;
    }

    private void Update()
    {
        if (!isGrabbing || grabbingHand == null) return;

        Quaternion currentHandRotation = grabbingHand.transform.rotation;
        Quaternion deltaRotation = currentHandRotation * Quaternion.Inverse(initialHandRotation);

        Vector3 deltaEuler = deltaRotation.eulerAngles;
        float angleZ = Mathf.DeltaAngle(0, deltaEuler.z); 

        Debug.Log($"grab: {angleZ}");
        

        int direction = angleZ > 0 ? 1 : -1;

        radioDialHandler.RotateDial(Mathf.Abs(angleZ), direction);
        initialHandRotation = currentHandRotation;
    }
}
