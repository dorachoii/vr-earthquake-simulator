using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

// grib�� ������ �� ��ü�� �� ���� �ڽ� ������Ʈ�� ���´�.
// ���� �� ��ü�� �߷��� ������ �޾� �������� �ȵǴϱ� rigidbody�� ����
// ������ ��ü�� forward������ ���� forward�������� �����ش�.

// grib�� �� �� �� ������ rigidbody�� �Ѱ�
// �θ� ������Ʈ�� null�� ���� �� �ڸ����� �������� �Ѵ�.

public class EJLeftHandGrab : MonoBehaviour
{
    //public GameObject LHand;
    public GameObject LHand;
    LineRenderer Lline;
    LineRenderer RLine;

    public GameObject grabPocket;
    Vector3 originSize;

    public GameObject valveTrigger;

    LayerMask movableProps;
    int grabObj;

    #region VRInput Variables

    public XRIDefaultInputActions inputActions;
    public InputAction XRButtonPrimary;
    public InputAction XRButtonSecondary;
    public InputAction XRButtonTrigger;
    public InputAction XRButtonGrip;

    #endregion

    private void Awake()
    {
        inputActions = new XRIDefaultInputActions();
        RLine = LHand.GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
        XRButtonPrimary = inputActions.EJXRInput.PrimaryBtn;
        XRButtonSecondary = inputActions.EJXRInput.SecondaryBtn;
        XRButtonGrip = inputActions.EJXRInput.GripBtn;
        XRButtonTrigger = inputActions.EJXRInput.TriggerBtn;


        XRButtonPrimary.Enable();
        XRButtonSecondary.Enable();
        XRButtonGrip.Enable();
        XRButtonTrigger.Enable();

        XRButtonGrip.started += OnStarted;
        XRButtonGrip.canceled += OnCanceled;
    }
    private void OnDisable()
    {
        XRButtonGrip.started -= OnStarted;
        XRButtonGrip.canceled -= OnCanceled;
    }
    enum HandState
    {
        None,
        Pressed,
        Pressing,
        Released,
    }

    HandState handState;
    void OnStarted(CallbackContext context)
    {
        RLine.enabled = true;
        handState = HandState.Pressed;
    }

    void OnCanceled(CallbackContext context)
    {
        RLine.enabled = false;
        handState = HandState.Released;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Lline = LHand.GetComponent<LineRenderer>();
        movableProps = 1 << LayerMask.NameToLayer("interactiveProps");
        grabObj = 1 << LayerMask.NameToLayer("movableProps") | 1 << LayerMask.NameToLayer("obstacles");
    }

    

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(LHand.transform.position, LHand.transform.forward);
        RaycastHit hit;

        ClickByRay();
    }

    bool isGrabbed;
    GameObject grabObject;

    private void ClickByRay()
    {
        Ray ray = new Ray(LHand.transform.position, LHand.transform.forward);
        RaycastHit hit;

        // �̵��� Lerp�� �ε巴�� ������ ����������.
        if (handState == HandState.Pressing && grabObject)
        {
            //grabObject.transform.localScale = originSize;
            grabObject.transform.position = Vector3.Lerp(grabObject.transform.position, LHand.transform.position + LHand.transform.forward*1.1f, Time.deltaTime * 10);

        }

        if (Physics.Raycast(ray, out hit, 10f, /*interactivePropsLayer*/grabObj))
        {
            //print("hit�� ���� ����" + hit.transform.gameObject.name);

            if (handState == HandState.Pressed && !isGrabbed)
            {
                if (hit.transform.gameObject.name.Contains("ipad"))
                {
                    EJToDoManager.instance.ONcheckTablet();
                    
                }


                // grib�� ������ �� ��ü�� �� ���� �ڽ� ������Ʈ�� ���´�.
                hit.transform.gameObject.transform.parent = grabPocket.transform;
                //hit.transform.localPosition = Vector3.one;
                //hit.transform.localScale = Vector3.one * 0.1f;
                print("hit�� parent��" + hit.transform.gameObject.transform.parent);

                isGrabbed = true;

                // ���� �� ��ü�� �߷��� ������ �޾� �������� �ȵǴϱ� rigidbody�� ����
                if (null != hit.transform.gameObject.GetComponent<Rigidbody>())
                {
                    hit.transform.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    hit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }


                // ������ ��ü�� forward������ ���� forward����? mainCam�� forward �������� �����ش�.
                hit.transform.forward = - Camera.main.transform.forward;
                grabObject = hit.transform.gameObject;

                if (hit.transform.gameObject.name.Contains("velve2")&&XRButtonPrimary.triggered)
                {
                    print("valve22222");
     
                        valveTrigger.GetComponent<EJGrabGasvelveTrigger>().ONMeshvalveTrigger();
                    
                }
          
            }
        }
        else
        {
            // ���
        }

        if (handState == HandState.Released)
        {
            if (null != grabObject)
            {
                // grib�� �� �� �� ������ rigidbody�� �Ѱ�
                //LHand.transform.gameObject.GetComponent<MeshCollider>().enabled = true;
                if (null != grabObject.transform.gameObject.GetComponent<Rigidbody>())
                {
                    grabObject.transform.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    grabObject.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                //ƨ��� �ɷ� �ؾ���


                // �θ� ������Ʈ�� null�� ���� �� �ڸ����� �������� �Ѵ�.
                grabObject.transform.gameObject.transform.parent = null;
                //grabObject.transform.localScale *= 10; /** 10f*/ //��ü�� ����ġ �ʿ�;

                isGrabbed = false;
                grabObject = null;

                //LHand.transform.gameObject.GetComponent<MeshCollider>().enabled = false;
            }
        }


        if (handState == HandState.Pressed)
            handState = HandState.Pressing;
        else if (handState == HandState.Released)
            handState = HandState.None;
    }
}
