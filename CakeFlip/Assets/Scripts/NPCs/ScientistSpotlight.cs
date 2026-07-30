using System.Collections;
using UnityEngine;

public class ScientistSpotlight : MonoBehaviour
{
    [Header("Spotlight")]
    [SerializeField] private Light spotlight;
    [SerializeField] private float maxSpotlightIntensity = 110f;
    [SerializeField] private bool spotlightEnabled = false;
    [SerializeField] private float spotlightDelay = .5f;

    [Header("Pass off to Dialogue")]
    [SerializeField] private float dialogueDelay = 1f;
    [SerializeField] private ScientistDialogue dialogue;

    private void Start()
    {
        if (dialogue == null)
        {
            dialogue = GetComponent<ScientistDialogue>();
        }
        //fade in the spotlight
        StartCoroutine(nameof(EnableSpotlight));
    }

    private IEnumerator EnableSpotlight()
    {
        yield return new WaitForSeconds(spotlightDelay);
        spotlightEnabled = true;
        yield return new WaitForSeconds(dialogueDelay);
        //Dialogue.gameObject.SetActive(true);
        dialogue.InitialWakeupDialogue();
    }

    private void Update()
    {
        if (spotlightEnabled && spotlight != null)
        {
            spotlight.intensity = Mathf.Lerp(spotlight.intensity, maxSpotlightIntensity, Time.deltaTime);
        }
    }
}
