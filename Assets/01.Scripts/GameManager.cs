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
        SetMission(MissionState.debris);
    }

    public void SetGameState(GameState state)
    {
        GameState previousState = currentGameState;
        currentGameState = state;

        // 상태 변경에 따른 흔들림 제어
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

    /// <summary>
    /// 게임 상태 변경에 따른 흔들림을 처리합니다.
    /// </summary>
    private void HandleShakeOnStateChange(GameState previousState, GameState newState)
    {
        if (shakeManager == null) return;

        switch (newState)
        {
            case GameState.Earthquake:
                if (enableShakeOnEarthquake)
                {
                    // 지진 상태일 때 흔들림 시작
                    shakeManager.SetShakeSettings(earthquakeShakeDuration, earthquakeShakeIntensity, earthquakeShakeFrequency);
                    shakeManager.StartShake();
                    Debug.Log("Earthquake shake started!");
                }
                break;

            case GameState.Aftershock:
                if (enableShakeOnEarthquake)
                {
                    // 여진 상태일 때 약한 흔들림
                    shakeManager.SetShakeSettings(earthquakeShakeDuration * 0.5f, earthquakeShakeIntensity * 0.3f, earthquakeShakeFrequency * 0.7f);
                    shakeManager.StartShake();
                    Debug.Log("Aftershock shake started!");
                }
                break;

            case GameState.Escape:
                // 탈출 상태일 때 흔들림 중지
                shakeManager.StopShake();
                Debug.Log("Shake stopped for escape state!");
                break;
        }
    }

    /// <summary>
    /// 수동으로 흔들림을 시작합니다.
    /// </summary>
    public void StartShakeManually()
    {
        if (shakeManager != null)
        {
            shakeManager.StartShake();
        }
    }

    /// <summary>
    /// 수동으로 흔들림을 중지합니다.
    /// </summary>
    public void StopShakeManually()
    {
        if (shakeManager != null)
        {
            shakeManager.StopShake();
        }
    }

    /// <summary>
    /// 현재 흔들림 상태를 반환합니다.
    /// </summary>
    public bool IsShaking()
    {
        return shakeManager != null && shakeManager.IsShaking();
    }

    /// <summary>
    /// 남은 흔들림 시간을 반환합니다.
    /// </summary>
    public float GetRemainingShakeTime()
    {
        return shakeManager != null ? shakeManager.GetRemainingShakeTime() : 0f;
    }
}
