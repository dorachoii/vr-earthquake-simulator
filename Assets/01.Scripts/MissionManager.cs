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

    private void OnEnable()
    {
        //ClickItemHandler.OnClickItemCompleted += HandleClickItemComplete;
    }

    void OnDisable()
    {
        //ClickItemHandler.OnClickItemCompleted -= HandleClickItemComplete;
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

    // void HandleClickItemComplete(ClickItemHandler.ClickItemType type)
    // {

    //     if (GetCurrentMissionState().ToString().ToLower() == type.ToString().ToLower())
    //     {
    //         CompleteCurrentMission();
    //     }
    // }

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
    }


    void UpdateMissionUI()
    {
        if (missionUIManager != null)
        {
            missionUIManager.UpdateMissionUI(missions, currentMissionIndex);
        }
    }

    void CreateDefaultMissions()
    {
        missions.Clear();

        missions.Add(new MissionData { missionState = MissionState.slippers, missionText = "Find the Slipper" });
        missions.Add(new MissionData { missionState = MissionState.radio, missionText = "Listen Radio" });
        missions.Add(new MissionData { missionState = MissionState.fusebox, missionText = "Check Fusebox" });
        missions.Add(new MissionData { missionState = MissionState.door, missionText = "Open Door" });
        missions.Add(new MissionData { missionState = MissionState.gasVelve, missionText = "Turn off Gas Valve" });

        missions.Add(new MissionData { missionState = MissionState.flashlight, missionText = "Get Flashlight" });
        missions.Add(new MissionData { missionState = MissionState.tablet, missionText = "Check Tablet" });
        missions.Add(new MissionData { missionState = MissionState.Escape, missionText = "Escape!" });
        missions.Add(new MissionData { missionState = MissionState.Complete, missionText = "Mission Complete!" });
    }

    public void CompleteCurrentMission()
    {
        if (currentMissionIndex < missions.Count)
        {
            missions[currentMissionIndex].isCompleted = true;
            NextMission();
        }
    }

    public void NextMission()
    {
        currentMissionIndex++;

        if (currentMissionIndex < missions.Count)
        {
            GameManager.Instance.SetMission(missions[currentMissionIndex].missionState);
            UpdateMissionUI();
        }
        else
        {
            GameManager.Instance.SetGameState(GameState.Escape);
        }
    }


    public MissionState GetCurrentMissionState()
    {
        if (currentMissionIndex < missions.Count)
        {
            return missions[currentMissionIndex].missionState;
        }
        return MissionState.Complete;
    }


}