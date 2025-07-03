using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MissionUITrigger : MonoBehaviour
{
    private GameObject uiPanel;
    private MissionState thisMission;
    private string canvasName;

    void Start()
    {
        string[] parts = gameObject.name.Split('_');

        if (parts.Length >= 3)
        {
            string keyword = parts[2].ToLower();

            foreach (MissionState mission in System.Enum.GetValues(typeof(MissionState)))
            {
                if (mission.ToString().ToLower().Contains(keyword))
                {
                    thisMission = mission;
                    break;
                }
            }
        }

        canvasName = $"{parts[0]}_Canvas_{parts[2]}";
        var found = GameObject.Find(canvasName);
        if (found != null)
        {
            var child = found.transform.childCount > 0 ? found.transform.GetChild(0).gameObject : null;
            uiPanel = child ?? found;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{gameObject.name}에 {other.name}이 부딪혔고 현재 미션은 {GameManager.Instance.currentMission}이며, 이 오브젝트의 미션은 {thisMission}, 캔버스는 {canvasName}");

        if (other.CompareTag("Player") && GameManager.Instance.currentMission == thisMission)
        {
            uiPanel?.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.currentMission == thisMission)
        {
            uiPanel?.SetActive(false);
        }
    }
}