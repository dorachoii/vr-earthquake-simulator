using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreRotation : MonoBehaviour
{
    private Transform parent;
    private float fixedY;
    private Quaternion initialLocalRotation;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        fixedY = transform.position.y;
        initialLocalRotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (parent == null) return;

        Vector3 targetXZ = parent.position;
        transform.position = new Vector3(targetXZ.x, fixedY, targetXZ.z);
        transform.rotation = Quaternion.identity;
    }
}
