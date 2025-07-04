using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class RadioDialHandler : MonoBehaviour
{
    public Transform dialTransform;
    public Transform gridLine;

    private float gridLineX = 0.03f;
    private const float gridMinX = -0.05f;
    private const float gridMaxX = 0.04f;

    private float stayTimer = 0f;
    private bool isCompleted = false;
    private readonly float successDuration = 0.1f;


    public void RotateDial(float angle, int direction)
    {
        if (isCompleted) return;

        float rotationAmount = angle * direction;
        dialTransform.Rotate(0, 0, rotationAmount);

        float gridOffset = rotationAmount * 0.001f;
        gridLineX -= gridOffset;

        gridLineX = Mathf.Clamp(gridLineX, gridMinX, gridMaxX);
        Vector3 pos = gridLine.localPosition;
        gridLine.localPosition = new Vector3(gridLineX, pos.y, pos.z);

        CheckSuccess();
    }

    private void CheckSuccess()
    {
        if (gridLineX >= -0.03f && gridLineX <= -0.02f)
        {
            stayTimer += Time.deltaTime;

            if (stayTimer >= successDuration && !isCompleted)
            {
                gridLine.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                MissionManager.Instance.CompleteCurrentMission();
                isCompleted = true;

                dialTransform.rotation = Quaternion.identity;
            }
        }
        else
        {
            stayTimer = 0f;
        }
    }
    
}
