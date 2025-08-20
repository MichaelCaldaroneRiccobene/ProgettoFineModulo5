using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShowOrHide : MonoBehaviour
{
    private Coroutine coroutine;
    public void ShowOrHideUI(CanvasGroup canvasGroup, float velocity, float startPoint, float endPoint)
    {
        if(coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(AToBRoutine(canvasGroup, velocity, startPoint, endPoint));
    }

    private IEnumerator AToBRoutine(CanvasGroup canvasGroup, float velocity, float startPoint, float endPoint)
    {
        float currentA = startPoint;
        if (velocity <= 0) velocity = 1;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * velocity;

            canvasGroup.alpha = Mathf.Lerp(currentA, endPoint, progress);
            yield return null;
        }
        canvasGroup.alpha = endPoint;
    }
}
