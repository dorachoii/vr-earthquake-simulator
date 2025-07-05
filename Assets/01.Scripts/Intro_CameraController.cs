using System.Collections;
using UnityEngine;

public enum ShakeMode
{
    IntroMovingShake, // 초반 카메라 이동 + 흔들림
    StaticShake       // 여진 중 고정 위치에서 흔들림
}

public class Intro_CameraController : MonoBehaviour
{
    [Header("Cameras")]
    public Camera shakeCam;
    public Camera mainCam;

    [Header("Movement (Intro Only)")]
    public GameObject stopPoint;
    public float moveSpeed = 6.0f;

    [Header("Shake Settings")]
    [Range(0.01f, 0.1f)] public float shakeRange = 0.05f;
    [Range(1f, 5f)] public float shakeDuration = 3f;

    [Header("Fade Effect")]
    public Intro_ScreenFader fader;

    [Header("Shake Mode")]
    public ShakeMode mode = ShakeMode.IntroMovingShake;

    private bool isShaking = false;
    private float elapsedShakeTime = 0f;
    private Vector3 originalShakeCamOffset;

    void Awake()
    {
        if (mode == ShakeMode.StaticShake)
        {
            enabled = false; // 정적 모드는 GameState 이벤트로 실행되므로 초기 비활성화
        }
    }

    void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    void Start()
    {
        if (mode == ShakeMode.IntroMovingShake)
        {
            shakeCam.gameObject.SetActive(true);
            mainCam?.gameObject.SetActive(false);
            originalShakeCamOffset = shakeCam.transform.localPosition - transform.position;
            isShaking = true;
        }
        else
        {
            shakeCam.gameObject.SetActive(false);
            originalShakeCamOffset = Vector3.zero;
        }

        elapsedShakeTime = 0f;
    }

    void Update()
    {
        if (!isShaking) return;

        elapsedShakeTime += Time.deltaTime;

        if (mode == ShakeMode.IntroMovingShake)
        {
            // 이동하면서 흔들림
            transform.position = Vector3.MoveTowards(transform.position, stopPoint.transform.position, moveSpeed * Time.deltaTime);
        }

        ApplyShake();

        if (elapsedShakeTime >= shakeDuration)
        {
            isShaking = false;

            if (mode == ShakeMode.IntroMovingShake)
            {
                StartCoroutine(DoCameraSwitchWithFade());
            }
            else
            {
                shakeCam.gameObject.SetActive(false); // 여진 끝나면 그냥 꺼짐
            }

            enabled = false;
        }
    }

    private void ApplyShake()
    {
        float offsetX = Random.Range(-shakeRange, shakeRange);
        float offsetY = Random.Range(-shakeRange, shakeRange);
        Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0);

        shakeCam.transform.position = transform.position + originalShakeCamOffset + shakeOffset;
    }

    IEnumerator DoCameraSwitchWithFade()
    {
        if (fader != null)
        {
            fader.StartFadeOut();
            yield return new WaitForSeconds(fader.fadeDuration);
        }

        shakeCam.gameObject.SetActive(false);
        mainCam?.gameObject.SetActive(true);

        if (fader != null)
        {
            fader.StartFadeIn();
        }
    }

    private void HandleGameStateChanged(GameState newState)
    {
        Debug.Log("IntroCameraControl- HandleGmaeState");
        
        if (newState == GameState.Aftershock && mode == ShakeMode.StaticShake)
        {
            elapsedShakeTime = 0f;
            isShaking = true;
            enabled = true;

            shakeCam.gameObject.SetActive(true);
        }
    }
}
