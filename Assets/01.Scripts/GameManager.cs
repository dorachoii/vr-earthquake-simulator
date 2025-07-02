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

    [Header("Shake Management")]
    [SerializeField] private bool enableShakeOnEarthquake = true;
    [SerializeField] private float earthquakeShakeDuration = 10f;
    [SerializeField] private float earthquakeShakeIntensity = 0.15f;
    [SerializeField] private float earthquakeShakeFrequency = 15f;

    private ShakeManager shakeManager;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // ShakeManager 찾기
        shakeManager = FindObjectOfType<ShakeManager>();
        if (shakeManager == null)
        {
            Debug.LogWarning("ShakeManager not found in scene!");
        }

        SetGameState(GameState.Earthquake);
        SetMission(MissionState.slippers);
    }

    public void SetGameState(GameState state)
    {
        GameState previousState = currentGameState;
        currentGameState = state;

        HandleShakeOnStateChange(previousState, state);
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

    private void HandleShakeOnStateChange(GameState previousState, GameState newState)
    {
        if (shakeManager == null) return;

        switch (newState)
        {
            case GameState.Earthquake:
                if (enableShakeOnEarthquake)
                {
                    shakeManager.SetShakeSettings(earthquakeShakeDuration, earthquakeShakeIntensity, earthquakeShakeFrequency);
                    shakeManager.StartShake();
                    Debug.Log("Earthquake shake started!");
                }
                break;

            case GameState.Aftershock:
                if (enableShakeOnEarthquake)
                {
                    shakeManager.SetShakeSettings(earthquakeShakeDuration * 0.5f, earthquakeShakeIntensity * 0.3f, earthquakeShakeFrequency * 0.7f);
                    shakeManager.StartShake();
                    Debug.Log("Aftershock shake started!");
                }
                break;

            case GameState.Escape:
                shakeManager.StopShake();
                Debug.Log("Shake stopped for escape state!");
                break;
        }
    }

    public void StartShakeManually()
    {
        if (shakeManager != null)
        {
            shakeManager.StartShake();
        }
    }

    public void StopShakeManually()
    {
        if (shakeManager != null)
        {
            shakeManager.StopShake();
        }
    }
    public bool IsShaking()
    {
        return shakeManager != null && shakeManager.IsShaking();
    }

    public float GetRemainingShakeTime()
    {
        return shakeManager != null ? shakeManager.GetRemainingShakeTime() : 0f;
    }
}
