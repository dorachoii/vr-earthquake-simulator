using UnityEngine;

public class RadioDialHandler : MonoBehaviour
{
    public Transform dialTransform;
    public Transform gridLine;

    private float gridLineX = 0.03f; 
    private const float gridMinX = -0.05f;
    private const float gridMaxX = 0.04f;

    public void RotateDial(float angle, int direction)
    {
        float rotationAmount = angle * direction;
        dialTransform.Rotate(0, 0, rotationAmount);

        float gridOffset = rotationAmount * 0.001f; 
        gridLineX -= gridOffset;

        gridLineX = Mathf.Clamp(gridLineX, gridMinX, gridMaxX);
        Vector3 pos = gridLine.localPosition;
        gridLine.localPosition = new Vector3(gridLineX, pos.y, pos.z);
    }
}
