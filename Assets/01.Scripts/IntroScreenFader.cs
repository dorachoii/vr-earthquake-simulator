using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreenFader : MonoBehaviour
{
    Image fadeImage;
    public float fadeDuration = 1.5f;

    Coroutine currentFade;

    void Start()
    {
        fadeImage = GetComponent<Image>();
    }
    public void StartFadeIn()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(Fade(1, 0));
    }

    public void StartFadeOut()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(Fade(0, 1));
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, to);
        currentFade = null;
    }
}
