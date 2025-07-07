using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HingedDoor : MonoBehaviour
{
    Transform targetToRotate;

    public Vector3 rotationOffset = new Vector3(100, 0, 0);
    public float rotationSpeed = 2f;

    [Header("Light Control")]
    public GameObject[] lights;

    public Action onOpend;
    public Action onClosed;

    private Quaternion startRot;
    private Quaternion endRot;
    private bool isOpen = false;

    public bool IsOpen => isOpen;

    private void Start()
    {
        if (targetToRotate == null)
        {
            targetToRotate = transform;
        }

        startRot = targetToRotate.localRotation;
        endRot = startRot * Quaternion.Euler(rotationOffset);
    }

    public void Toggle()
    {
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine() {
        Quaternion from = isOpen ? endRot : startRot;
        Quaternion to = isOpen ? startRot : endRot;

        while (Quaternion.Angle(targetToRotate.localRotation, to) > 0.5f)
        {
            targetToRotate.localRotation = Quaternion.Lerp(targetToRotate.localRotation, to, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        targetToRotate.localRotation = to;
        isOpen = !isOpen;

        if (isOpen) 
        {
            onOpend?.Invoke();
            TurnOffLights();
        }
        else 
        {
            onClosed?.Invoke();
            TurnOnLights();
        }
    }

    private void TurnOffLights()
    {
        if (lights == null || lights.Length == 0) return;
        
        foreach (var obj in lights)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    private void TurnOnLights()
    {
        if (lights == null || lights.Length == 0) return;
        
        foreach (var obj in lights)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
