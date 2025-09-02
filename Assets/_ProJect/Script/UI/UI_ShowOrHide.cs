using System;
using System.Collections;
using UnityEngine;

public class UI_ShowOrHide : MonoBehaviour
{
    private Coroutine coroutine;
    public void ShowOrHideUI(CanvasGroup canvasGroup, float velocity, float startPoint, float endPoint)
    {
        if (canvasGroup != null) ShowOrHideWithActionUI(canvasGroup, velocity, startPoint, endPoint, null);
    }

    public void ShowOrHideWithActionUI(CanvasGroup canvasGroup, float velocity, float startPoint, float endPoint, Action onComplete = null)
    {
        if (coroutine != null) StopCoroutine(coroutine);

        if (canvasGroup != null) coroutine = StartCoroutine(AToBRoutine(canvasGroup, velocity, startPoint, endPoint, onComplete));
    }

    private IEnumerator AToBRoutine(CanvasGroup canvasGroup, float velocity, float startPoint, float endPoint, Action onComplete)
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

        onComplete?.Invoke();
    }
}
