using UnityEngine;
using TMPro;

public enum GameState
{
    Earthquake,
    Aftershock, //여진
    Escape
}

public enum MissionState
{
    debris,
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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetGameState(GameState.Earthquake);
        SetMission(MissionState.debris);
    }

    public void SetGameState(GameState state)
    {
        currentGameState = state;
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
