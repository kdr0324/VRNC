using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    private CanvasGroup cg;
    public float fadeTime = 1f;
    float accumTime = 0f;

    private Coroutine fadeCor;

    private void Awake()
    {
        cg = gameObject.GetComponent<CanvasGroup>();
    }

    public void Fade()
    {
        StartFadeIn();
        StartFadeOut();
    }

    public void StartFadeIn()
    {
        if (fadeCor != null)
        {
            StopAllCoroutines();
            fadeCor = null;
        }
        fadeCor = StartCoroutine(FadeIn());

    }

    public void StartFadeOut()
    {
        if (fadeCor != null)
        {
            StopAllCoroutines();
            fadeCor = null;
        }
        fadeCor = StartCoroutine(FadeOut());

    }

    private IEnumerator FadeIn()
    {
        accumTime = 0f;
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 1f;
    }


    private IEnumerator FadeOut()
    {
        accumTime = 0f;
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 0f;
    }

}
