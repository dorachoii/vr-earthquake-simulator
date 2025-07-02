using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InputTester : MonoBehaviour
{
    ActionBasedController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        var select = controller.selectAction.action;
        if (select != null && select.IsPressed())
        {
            Debug.Log("test: Select 버튼");
        }

        var activate = controller.activateAction.action;
        if (activate != null && activate.WasPressedThisFrame())
        {
            Debug.Log("test: Activate 버튼");
        }
    }
}
