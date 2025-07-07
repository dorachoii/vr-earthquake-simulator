using System.Collections;
using UnityEngine;

public class Intro_CameraController : MonoBehaviour
{
    public Camera shakeCam;

    [Header("Movement")]
    public GameObject stopPoint;
    public float moveSpeed = 6.0f;

    [Header("Shake Settings")]
    [Range(0.01f, 0.1f)] public float shakeRange = 0.05f;
    [Range(1f, 5f)] public float shakeDuration = 3f;

    [Header("Fade Effect")]
    public Intro_ScreenFader fader;

    private bool isMoving = false;
    private bool isShaking = false;
    private bool hasReachedDestination = false;
    private float elapsedShakeTime = 0f;
    private Vector3 originalShakeCamOffset;

    void Start()
    {
        shakeCam.gameObject.SetActive(true);
        originalShakeCamOffset = shakeCam.transform.localPosition - transform.position;
        isMoving = true;
        isShaking = true;
        elapsedShakeTime = 0f;
    }

    void Update()
    {
        if (!isMoving || hasReachedDestination) return;

        transform.position = Vector3.MoveTowards(transform.position, stopPoint.transform.position, moveSpeed * Time.deltaTime);

        if (isShaking)
        {
            elapsedShakeTime += Time.deltaTime;
            ApplyShake();

            if (elapsedShakeTime >= shakeDuration)
            {
                isShaking = false;
            }
        }

        if (Vector3.Distance(transform.position, stopPoint.transform.position) < 0.1f)
        {
            hasReachedDestination = true;
            isMoving = false;

            GameManager.Instance.SetGameState(GameState.Mission);
            
            StartCoroutine(DoCameraSwitchWithFade());
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

        if (fader != null)
        {
            fader.StartFadeIn();
        }
    }
}
