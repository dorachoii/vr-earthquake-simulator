using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class RadioDialHandler : MonoBehaviour
{
    public Transform dialTransform;
    public Transform gridLine;

    public GameObject powerBtn;
    public AudioSource broadCast;
    public AudioSource noise;
    
    [Header("Audio Settings")]
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private float minVolume = 0f;
    [SerializeField] private float correctFrequencyMin = -0.03f;
    [SerializeField] private float correctFrequencyMax = -0.02f;

    private float gridLineX = 0.03f;
    private const float gridMinX = -0.05f;
    private const float gridMaxX = 0.04f;

    private float stayTimer = 0f;
    private bool isCompleted = false;
    private bool isRadioOn = false;
    private readonly float successDuration = 0.1f;

    void Start()
    {
        // 초기 상태: 라디오 꺼짐
        SetAudioSources(false);
    }

    public void TurnOnRadio()
    {
        powerBtn.SetActive(true);
        isRadioOn = true;
        SetAudioSources(true);
    }

    public void RotateDial(float angle, int direction)
    {
        if (isCompleted) return;

        float rotationAmount = angle * direction;
        dialTransform.Rotate(0, 0, rotationAmount);

        float gridOffset = rotationAmount * 0.001f;
        gridLineX -= gridOffset;

        gridLineX = Mathf.Clamp(gridLineX, gridMinX, gridMaxX);
        Vector3 pos = gridLine.localPosition;
        gridLine.localPosition = new Vector3(gridLineX, pos.y, pos.z);

        // 주파수에 따른 오디오 볼륨 조절
        if (isRadioOn)
        {
            UpdateAudioVolumes();
        }

        CheckSuccess();
    }

    private void UpdateAudioVolumes()
    {
        // 올바른 주파수 범위까지의 거리 계산
        float distanceToCorrect = GetDistanceToCorrectFrequency();
        
        // 전체 주파수 범위에서의 정규화된 거리 (0~1)
        float normalizedDistance = distanceToCorrect / (gridMaxX - gridMinX);
        
        // 방송 볼륨: 거리가 가까울수록 크게
        float broadcastVolume = Mathf.Lerp(maxVolume, minVolume, normalizedDistance);
        broadCast.volume = broadcastVolume;
        
        // 노이즈 볼륨: 거리가 멀수록 크게
        float noiseVolume = Mathf.Lerp(minVolume, maxVolume, normalizedDistance);
        noise.volume = noiseVolume;
    }

    private float GetDistanceToCorrectFrequency()
    {
        if (gridLineX >= correctFrequencyMin && gridLineX <= correctFrequencyMax)
        {
            return 0f; // 정확한 주파수
        }
        
        if (gridLineX < correctFrequencyMin)
        {
            return correctFrequencyMin - gridLineX;
        }
        else
        {
            return gridLineX - correctFrequencyMax;
        }
    }

    private void SetAudioSources(bool isOn)
    {
        if (isOn)
        {
            if (!broadCast.isPlaying) broadCast.Play();
            if (!noise.isPlaying) noise.Play();
        }
        else
        {
            if (broadCast.isPlaying) broadCast.Stop();
            if (noise.isPlaying) noise.Stop();
        }
    }

    private void CheckSuccess()
    {
        if (gridLineX >= correctFrequencyMin && gridLineX <= correctFrequencyMax)
        {
            stayTimer += Time.deltaTime;

            if (stayTimer >= successDuration && !isCompleted)
            {
                gridLine.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                MissionManager.Instance.CompleteMission(MissionState.radio);
                isCompleted = true;

                dialTransform.rotation = Quaternion.identity;
            }
        }
        else
        {
            stayTimer = 0f;
        }
    }
}
