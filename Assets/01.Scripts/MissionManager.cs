using UnityEngine;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class MissionData
{
    public MissionState missionState;
    public string missionText;
    public GameObject missionTrigger;
    public bool isCompleted;
}

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Mission Data")]
    [SerializeField] private List<MissionData> missions = new List<MissionData>();

    [SerializeField] private MissionUIManager missionUIManager;

    private int currentMissionIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeMissions();

        if (missionUIManager != null)
        {
            var initialMissions = missions.GetRange(0, Mathf.Min(5, missions.Count));
            missionUIManager.InitializeMissionUI(initialMissions);
        }

        UpdateMissionUI();
    }

    void InitializeMissions()
    {
        if (missions.Count == 0)
        {
            CreateDefaultMissions();
        }

        for (int i = 0; i < missions.Count; i++)
        {
            missions[i].isCompleted = false;
        }

        // 초기 상태 전달
        if (missions.Count > 0)
            GameManager.Instance.SetMission(missions[0].missionState);
    }

    void CreateDefaultMissions()
    {
        missions.Clear();

        missions.Add(new MissionData { missionState = MissionState.slippers, missionText = "Find the Slipper" });
        missions.Add(new MissionData { missionState = MissionState.radio, missionText = "Listen to the Radio" });
        missions.Add(new MissionData { missionState = MissionState.fusebox, missionText = "Check the Fusebox" });
        missions.Add(new MissionData { missionState = MissionState.door, missionText = "Open the Door" });
        missions.Add(new MissionData { missionState = MissionState.gasVelve, missionText = "Turn off the Gas Valve" });

        missions.Add(new MissionData { missionState = MissionState.flashlight, missionText = "Get the Flashlight" });
        missions.Add(new MissionData { missionState = MissionState.tablet, missionText = "Check the Tablet" });
        missions.Add(new MissionData { missionState = MissionState.Escape, missionText = "Escape!" });
        missions.Add(new MissionData { missionState = MissionState.Complete, missionText = "Mission Complete!" });
    }

    /// <summary>
    /// 미션 상태에 해당하는 미션을 완료 처리
    /// </summary>
    public void CompleteMission(MissionState state)
    {
        foreach (var mission in missions)
        {
            if (mission.missionState == state && !mission.isCompleted)
            {
                mission.isCompleted = true;
                Debug.Log($"[MissionManager] Mission completed: {state}");
                UpdateMissionUI();
                TryAdvanceToNextMission();
                return;
            }
        }

        Debug.LogWarning($"[MissionManager] Mission not found or already completed: {state}");
    }

    private void TryAdvanceToNextMission()
    {
        while (currentMissionIndex < missions.Count && missions[currentMissionIndex].isCompleted)
        {
            currentMissionIndex++;
        }

        if (currentMissionIndex < missions.Count)
        {
            GameManager.Instance.SetMission(missions[currentMissionIndex].missionState);
        }
        else
        {
            GameManager.Instance.SetGameState(GameState.Escape);
        }
    }

    void UpdateMissionUI()
    {
        if (missionUIManager != null)
        {
            int currentIndex = GetFirstIncompleteMissionIndex();
            missionUIManager.UpdateMissionUI(missions, currentIndex);
        }
    }

    private int GetFirstIncompleteMissionIndex()
{
    for (int i = 0; i < missions.Count; i++)
    {
        if (!missions[i].isCompleted)
            return i;
    }
    return -1; 
}

}
