using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MissionUIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> missionEntries; // 부모 오브젝트들


    public void InitializeMissionUI(List<MissionData> initialMissions)
{
    for (int i = 0; i < missionEntries.Count; i++)
    {
        if (i >= initialMissions.Count) continue;

        var text = missionEntries[i].GetComponentInChildren<TextMeshProUGUI>();
        if (text == null) continue;

        text.text = initialMissions[i].missionText;
        text.alpha = 0.4f;
        text.fontStyle = FontStyles.Normal;
    }
}


    public void UpdateMissionUI(List<MissionData> missions, int currentIndex)
{
    for (int i = 0; i < missionEntries.Count; i++)
    {
        if (i >= missions.Count) continue;

        var text = missionEntries[i].GetComponentInChildren<TextMeshProUGUI>();
        if (text == null) continue;

        var mission = missions[i];
        text.text = mission.missionText;

        if (mission.isCompleted)
        {
            text.alpha = 0.4f;
            text.fontStyle = FontStyles.Strikethrough;
        }
        else if (i == currentIndex)
        {
            text.alpha = 1f;
            text.fontStyle = FontStyles.Normal;
        }
        else
        {
            text.alpha = 0.4f;
            text.fontStyle = FontStyles.Normal;
        }
    }
}

}
