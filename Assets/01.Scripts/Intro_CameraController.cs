using System.Collections;
using UnityEngine;

public class Intro_CameraController : MonoBehaviour
{
    public Camera shakeCam;
    public Camera mainCam;
    public GameObject stopPoint;
    public float moveSpeed = 6.0f;

    [Range(0.01f, 0.1f)] public float shakeRange = 0.05f;
    [Range(1f, 5f)] public float shakeDuration = 3f;

    private bool isMoving = true;
    private float elapsedShakeTime = 0f;
    private Vector3 originalShakeCamOffset;

    public Intro_ScreenFader fader;

    void Start()
    {
        shakeCam.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false);
        originalShakeCamOffset = shakeCam.transform.localPosition - transform.position;
    }

    void Update()
    {
        if (!isMoving) return;

        // 카메라 이동
        transform.position = Vector3.MoveTowards(transform.position, stopPoint.transform.position, moveSpeed * Time.deltaTime);

        // 흔들림
        float offsetX = Random.Range(-shakeRange, shakeRange);
        Vector3 shakenPos = transform.position + originalShakeCamOffset + new Vector3(offsetX, 0, 0);
        shakeCam.transform.position = shakenPos;

        elapsedShakeTime += Time.deltaTime;

        if (Vector3.Distance(transform.position, stopPoint.transform.position) < 0.01f || elapsedShakeTime > shakeDuration)
        {
            isMoving = false;
            StartCoroutine(DoCameraSwitchWithFade());
        }
    }

    IEnumerator DoCameraSwitchWithFade()
    {
        if (fader != null)
        {
            fader.StartFadeOut();
            yield return new WaitForSeconds(fader.fadeDuration);
        }

        shakeCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);

        if (fader != null)
        {
            fader.StartFadeIn();
        }
    }
}
