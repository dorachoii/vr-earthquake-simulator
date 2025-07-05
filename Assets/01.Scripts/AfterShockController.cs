using System.Collections;
using UnityEngine;

public class AfterShockController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject todoUI;
    public GameObject warningUI;

    [Header("Shake Settings")]
    public float shakeDuration = 3f;
    public float shakeIntensity = 0.05f;
    public float blinkInterval = 0.3f;

    private Coroutine blinkCoroutine;
    private Coroutine shakeCoroutine;

    private Vector3 originalCamPos;
    private Transform camTransform;

    void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Aftershock)
        {
            StartCoroutine(HandleAftershockSequence());
        }
    }

    private IEnumerator HandleAftershockSequence()
    {
        // 1. UI 처리
        if (todoUI) todoUI.SetActive(false);
        if (warningUI) warningUI.SetActive(true);

        // 2. 카메라 가져오기
        camTransform = Camera.main.transform;
        originalCamPos = camTransform.localPosition;

        // 3. 깜빡이기 & 흔들기 시작
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkWarning());

        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeCamera());

        // 4. 일정 시간 대기
        yield return new WaitForSeconds(shakeDuration);

        // 5. UI 복원
        if (todoUI) todoUI.SetActive(true);
        if (warningUI) warningUI.SetActive(false);

        // 6. 흔들림 & 깜빡이기 종료
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);

        // 7. 카메라 위치 복원
        if (camTransform != null)
            camTransform.localPosition = originalCamPos;

        MissionManager.Instance.AfterShocked();
    }

    private IEnumerator BlinkWarning()
    {
        while (true)
        {
            warningUI.SetActive(!warningUI.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator ShakeCamera()
    {
        while (true)
        {
            if (camTransform != null)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-shakeIntensity, shakeIntensity),
                    Random.Range(-shakeIntensity, shakeIntensity),
                    0f
                );
                camTransform.position = originalCamPos + offset;
            }

            yield return null; // 매 프레임마다 흔들기
        }
    }
}
