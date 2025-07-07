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
    }

    void CreateNewMissions()
    {
        missions.Clear();

        missions.Add(new MissionData { missionState = MissionState.flashlight, missionText = "Get the Flashlight" });
        missions.Add(new MissionData { missionState = MissionState.tablet, missionText = "Check the Tablet" });
        missions.Add(new MissionData { missionState = MissionState.Escape, missionText = "Escape!" });
    }

    public void AfterShocked()
    {
        // 여진 후 새로운 미션으로 교체
        CreateNewMissions();
        
        // 미션 인덱스 초기화
        currentMissionIndex = 0;

        // 초기 상태 전달
        if (missions.Count > 0)
            GameManager.Instance.SetMission(missions[0].missionState);

        if (missionUIManager != null)
        {
            var initialMissions = missions.GetRange(0, Mathf.Min(3, missions.Count));
            missionUIManager.InitializeMissionUI(initialMissions);
        }

        UpdateMissionUI();
    }

    public void CompleteMission(MissionState state)
    {
        foreach (var mission in missions)
        {
            if (mission.missionState == state && !mission.isCompleted)
            {
                mission.isCompleted = true;

                if (IsMainMissionsCompleted())
                {
                    Debug.Log("[MissionManager] Main 5 missions complete → Trigger Aftershock");
                    GameManager.Instance.SetGameState(GameState.Aftershock);
                }

                UpdateMissionUI();
                TryAdvanceToNextMission();
                return;
            }
        }
    }

    public void RevertMission(MissionState state)
    {
        foreach (var mission in missions)
        {
            if (mission.missionState == state && mission.isCompleted)
            {
                mission.isCompleted = false;
                UpdateMissionUI();
                TryAdvanceToNextMission();
                return;
            }
        }
    }

    private bool IsMainMissionsCompleted()
    {
        int count = Mathf.Min(5, missions.Count);
        for (int i = 0; i < count; i++)
        {
            if (!missions[i].isCompleted)
                return false;
        }
        return true;
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
