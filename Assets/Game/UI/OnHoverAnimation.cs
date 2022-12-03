using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class OnHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool useTime;
    [SerializeField] private float animationTime;
    [SerializeField] private float animationSpeed;
    [SerializeField] private AudioClip enterSound, exitSound;
    [SerializeField] private float randomPitch;
    [System.Serializable]
    private class Scale {
        public float scaleFactor;
    }
    [SerializeField] Scale scale;

    private Coroutine animationRoutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationRoutine != null) { StopCoroutine(animationRoutine); }
        animationRoutine = StartCoroutine(sizeAnimator(Vector3.one * scale.scaleFactor));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (animationRoutine != null) { StopCoroutine(animationRoutine); }
        animationRoutine = StartCoroutine(sizeAnimator(Vector3.one));
    }

    IEnumerator sizeAnimator(Vector3 scaleTo)
    {
        while (Mathf.Abs(transform.localScale.x - scaleTo.x) > 0.001f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleTo, Time.unscaledDeltaTime * animationSpeed);
            yield return null;
        }
    }
}
