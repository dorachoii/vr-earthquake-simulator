using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class EJHandActionTest : MonoBehaviour
{
    // Movable(click event �繰��)
    // secondaryBtn ������ Ray �߻� ��, primaryBtn ������ �۵�
    // slipper, fusebox, doors 

    // Grabbable
    // GrabBtn ������ ȸ���� ������ �ݿ�
    // ipad, velve, radio, flashlight

    public Transform LeftHand;
    public Transform RightHand;


    #region toolkit Input
    public XRIDefaultInputActions inputActions;
    public InputAction XRButtonPrimary;
    public InputAction XRButtonSecondary;
    public InputAction XRButtonTrigger;
    public InputAction XRButtonGrip;
    #endregion

    private void Awake()
    {
        inputActions = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        #region toolkit Input
        XRButtonPrimary = inputActions.EJXRInput.PrimaryBtn;
        XRButtonSecondary = inputActions.EJXRInput.SecondaryBtn;
        XRButtonGrip = inputActions.EJXRInput.GripBtn;
        XRButtonTrigger = inputActions.EJXRInput.TriggerBtn;

        XRButtonPrimary.Enable();
        XRButtonSecondary.Enable();
        XRButtonGrip.Enable();
        XRButtonTrigger.Enable();
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
