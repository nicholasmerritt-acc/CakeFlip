using System.Collections;
using UnityEngine;

public class ScientistSpotlight : MonoBehaviour
{
    [Header("Spotlight")]
    [SerializeField] private Light spotlight;
    [SerializeField] private float maxSpotlightIntensity = 110f;
    [SerializeField] private bool spotlightEnabled = false;
    [SerializeField] private float spotlightDelay = .5f;

    [Header("Dialogue")]
    [SerializeField] private float dialogueDelay = 1f;
    [SerializeField] private AudioClip welcomeBackClip;

    private void Start()
    {
        //fade in the spotlight
        StartCoroutine(nameof(EnableSpotlight));
    }

    private IEnumerator EnableSpotlight()
    {
        yield return new WaitForSeconds(spotlightDelay);
        spotlightEnabled = true;
        yield return new WaitForSeconds(dialogueDelay);
        GameManager.Instance.TheDialogueManager.SayNonBlockingDialogue("Welcome back.", welcomeBackClip);
    }

    private void Update()
    {
        if (spotlightEnabled && spotlight != null)
        {
            spotlight.intensity = Mathf.Lerp(spotlight.intensity, maxSpotlightIntensity, Time.deltaTime);
        }
    }
}
