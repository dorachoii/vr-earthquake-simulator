using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MissionItem : MonoBehaviour
{
    [Header("Mission Settings")]
    [SerializeField] private MissionState targetMission;
    [SerializeField] private bool destroyOnComplete = false;
    [SerializeField] private bool showDebugLogs = true;
    
    private XRSimpleInteractable interactable;
    private bool isCompleted = false;
    
    void Start()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        if (interactable == null)
        {
            interactable = gameObject.AddComponent<XRSimpleInteractable>();
        }
        
        interactable.selectEntered.AddListener(OnItemPicked);
    }
    
    private void OnItemPicked(SelectEnterEventArgs args)
    {
        if (isCompleted) return;
        
        if (MissionManager.Instance == null)
        {
            Debug.LogError("[MissionItem] MissionManager not found!");
            return;
        }
        
        if (MissionManager.Instance.GetCurrentMissionState() == targetMission)
        {
            CompleteMission();
        }
        else
        {
            if (showDebugLogs)
                Debug.Log($"[MissionItem] Current mission ({MissionManager.Instance.GetCurrentMissionState()}) doesn't match target mission ({targetMission})");
        }
    }
    
    void CompleteMission()
    {
        isCompleted = true;
        
        if (showDebugLogs)
            Debug.Log($"[MissionItem] Completed mission: {targetMission}");
        
        MissionManager.Instance.CompleteCurrentMission();
        
        if (destroyOnComplete)
        {
            Destroy(gameObject);
        }
    }
    
    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnItemPicked);
        }
    }
} 