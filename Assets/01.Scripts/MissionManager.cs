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
    
    [Header("Mission UI")]
    [SerializeField] private TextMeshProUGUI missionTextUI;
    
    [Header("Mission Data")]
    [SerializeField] private List<MissionData> missions = new List<MissionData>();
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;
    
    private int currentMissionIndex = 0;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        InitializeMissions();
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
        
        if (showDebugLogs)
            Debug.Log($"[MissionManager] Initialized {missions.Count} missions");
    }
    
    void CreateDefaultMissions()
    {
        missions.Clear();
        
        missions.Add(new MissionData { missionState = MissionState.debris, missionText = "Clear the debris" });
        missions.Add(new MissionData { missionState = MissionState.slippers, missionText = "Find the Slipper" });
        missions.Add(new MissionData { missionState = MissionState.radio, missionText = "Listen Radio" });
        missions.Add(new MissionData { missionState = MissionState.fusebox, missionText = "Check Fusebox" });
        missions.Add(new MissionData { missionState = MissionState.door, missionText = "Open Door" });
        missions.Add(new MissionData { missionState = MissionState.gasVelve, missionText = "Turn off Gas Valve" });
        missions.Add(new MissionData { missionState = MissionState.flashlight, missionText = "Get Flashlight" });
        missions.Add(new MissionData { missionState = MissionState.tablet, missionText = "Check Tablet" });
        missions.Add(new MissionData { missionState = MissionState.Escape, missionText = "Escape!" });
        missions.Add(new MissionData { missionState = MissionState.Complete, missionText = "Mission Complete!" });
        
        if (showDebugLogs)
            Debug.Log("[MissionManager] Created default missions");
    }
    
    public void CompleteCurrentMission()
    {
        if (currentMissionIndex < missions.Count)
        {
            missions[currentMissionIndex].isCompleted = true;
            
            if (showDebugLogs)
                Debug.Log($"[MissionManager] Completed mission: {missions[currentMissionIndex].missionText}");
            
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
            
            if (showDebugLogs)
                Debug.Log($"[MissionManager] Next mission: {missions[currentMissionIndex].missionText}");
        }
        else
        {
            GameManager.Instance.SetGameState(GameState.Escape);
            if (showDebugLogs)
                Debug.Log("[MissionManager] All missions completed!");
        }
    }
    
    public void SetMission(MissionState missionState)
    {
        for (int i = 0; i < missions.Count; i++)
        {
            if (missions[i].missionState == missionState)
            {
                currentMissionIndex = i;
                GameManager.Instance.SetMission(missionState);
                UpdateMissionUI();
                
                if (showDebugLogs)
                    Debug.Log($"[MissionManager] Set mission: {missions[i].missionText}");
                return;
            }
        }
    }
    
    void UpdateMissionUI()
    {
        if (missionTextUI != null && currentMissionIndex < missions.Count)
        {
            missionTextUI.text = missions[currentMissionIndex].missionText;
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
    
    public string GetCurrentMissionText()
    {
        if (currentMissionIndex < missions.Count)
        {
            return missions[currentMissionIndex].missionText;
        }
        return "Mission Complete!";
    }
    
    public bool IsMissionCompleted(MissionState missionState)
    {
        foreach (var mission in missions)
        {
            if (mission.missionState == missionState)
            {
                return mission.isCompleted;
            }
        }
        return false;
    }
    
    public void ResetMissions()
    {
        currentMissionIndex = 0;
        foreach (var mission in missions)
        {
            mission.isCompleted = false;
        }
        UpdateMissionUI();
        
        if (showDebugLogs)
            Debug.Log("[MissionManager] All missions reset");
    }
} 