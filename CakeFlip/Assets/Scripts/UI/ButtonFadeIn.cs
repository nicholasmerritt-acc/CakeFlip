using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonFadeIn : MonoBehaviour
{
    [Header("Fading")]
    [SerializeField] private float fadeDelay = 1.0f;
    [SerializeField] private float fadeSpeed = 1.0f;
    private float fadeIncrement;
    [SerializeField] private float startAlpha;

    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private RawImage image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        if (image == null)
        {
            image = GetComponent<RawImage>();
        }
        fadeIncrement = fadeSpeed / 100f;
        startAlpha = image.color.a;
        SetImageAlpha(0f);
        StartCoroutine(nameof(FadeImage));
    }

    private IEnumerator FadeImage()
    {
        yield return new WaitForSeconds(fadeDelay);
        float currentAlpha = 0f;
        while (currentAlpha < startAlpha)
        {
            currentAlpha += fadeIncrement;
            SetImageAlpha(currentAlpha);
            yield return new WaitForEndOfFrame();
        }
    }

    private void SetImageAlpha(float newAlpha)
    {
        var newColor = image.color;
        newColor.a = newAlpha;
        image.color = newColor;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
