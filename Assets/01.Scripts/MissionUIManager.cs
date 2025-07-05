using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MissionUIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> missionEntries; 


    public void InitializeMissionUI(List<MissionData> initialMissions)
{
    for (int i = 0; i < missionEntries.Count; i++)
    {
        var text = missionEntries[i].GetComponentInChildren<TextMeshProUGUI>();
        if (text == null) continue;

        if (i < initialMissions.Count)
        {
            // 새로운 미션이 있는 경우
            text.text = initialMissions[i].missionText;
            text.alpha = 0.4f;
            text.fontStyle = FontStyles.Normal;
            missionEntries[i].SetActive(true); // 칸 활성화
        }
        else
        {
            // 새로운 미션이 없는 경우 칸 숨기기
            missionEntries[i].SetActive(false);
        }
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
            // 완료된 미션: 파란색 + 취소선
            text.color = Color.blue;
            text.alpha = 0.9f;
            text.fontStyle = FontStyles.Strikethrough;
        }
        else if (i == currentIndex)
        {
            // 현재 우선순위 미션: 검정색 + 완전 불투명
            text.color = Color.black;
            text.alpha = 1f;
            text.fontStyle = FontStyles.Normal;
        }
        else
        {
            // 새로 생성된 미션: 검정색 + 약간 투명
            text.color = Color.black;
            text.alpha = 0.4f;
            text.fontStyle = FontStyles.Normal;
        }
    }
}

}
