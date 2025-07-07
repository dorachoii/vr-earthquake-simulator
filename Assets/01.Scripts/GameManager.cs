using UnityEngine;
using TMPro;

public enum GameState
{
    MainShock,
    Mission,
    Aftershock,
    Escape
}

public enum MissionState
{
    slippers,
    radio,
    fusebox,
    door,
    gasVelve,
    flashlight,
    tablet,
    Escape,
    Complete
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState currentGameState;
    public MissionState currentMission;

    public static event System.Action<GameState> OnGameStateChanged;

    private ShakeManager shakeManager;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        shakeManager = FindObjectOfType<ShakeManager>();
        if (shakeManager == null) return;

        SetGameState(GameState.MainShock);  
        SetMission(MissionState.slippers);
    }

    public void SetGameState(GameState state)
    {
        currentGameState = state;
        OnGameStateChanged?.Invoke(state);
    }

    public void SetMission(MissionState mission)
    {
        currentMission = mission;   
    }

    public void CompleteMission()
    {
        if (currentMission < MissionState.Complete)
        {
            SetMission(currentMission + 1);
        }
        else
        {
            SetGameState(GameState.Escape);
        }
    }
}
