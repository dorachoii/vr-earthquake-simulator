using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSimpleInteractable))]
public class ClickItemHandler : MonoBehaviour
{
    public enum ClickItemType { Slippers, Switch, Door }
    public ClickItemType type;

    XRSimpleInteractable interactable;

    void OnEnable()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnActivated);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnActivated);
    }

    void OnActivated(SelectEnterEventArgs args)
    {
        switch (type)
        {
            case ClickItemType.Slippers:
                HandleSlipperClick();
                break;
            case ClickItemType.Switch:
                break;
            case ClickItemType.Door:
                break;
        }
    }

    void HandleSlipperClick()
    {
        gameObject.SetActive(false);
        Debug.Log("슬리퍼 클릭됨!");
    }
}

