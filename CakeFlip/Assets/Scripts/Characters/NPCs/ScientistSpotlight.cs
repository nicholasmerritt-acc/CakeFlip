using System.Collections;
using UnityEngine;

/// <summary>
/// This nonsense is a little bit out of control. I should probably just delete it and make scientist a subclass of NPC.
/// However, it was one of the first parts of the game I worked on, and I'm feeling sentimental.
/// I'll delete it tomorrow.
/// </summary>
public class ScientistSpotlight : MonoBehaviour
{
    [Header("Spotlights")]
    [SerializeField] private Light scientistSpotlight;
    [SerializeField] private Light[] hallwaySpotlightPairs;
    [SerializeField] private float scientistSpotlightIntensity = 110f;
    [SerializeField] private float maxBigSpotlightIntensity = 1000f;
    [SerializeField] private float spotlightDelay = .5f;
    [SerializeField] private float scientistSpotlightFadeSpeed = 40f;
    [SerializeField] private float frontSpotlightFadeSpeed = 7f;
    [SerializeField] private float bigSpotlightFadeSpeed = 100f;

    [Header("Dialogue")]
    [SerializeField] private float dialogueDelay = 1f;
    [SerializeField] private AudioClip welcomeBackClip;

    private void Start()
    {
        StartCoroutine(nameof(EnableSpotlight));
    }

    private IEnumerator EnableSpotlight()
    {
        //fade in first spotlight, to illuminate Dr. Science
        yield return new WaitForSeconds(spotlightDelay);
        while (scientistSpotlight.intensity < scientistSpotlightIntensity)
        {
            scientistSpotlight.intensity += scientistSpotlightFadeSpeed * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(dialogueDelay);
        GameManager.Instance.TheDialogueManager.SayBlockingDialogue("Welcome back.", welcomeBackClip);

        if (hallwaySpotlightPairs == null || hallwaySpotlightPairs.Length == 0)
        {
            yield break;
        }

        //now, trigger the hallway lights one by one
        for (int spotlightIndexEveryOther = 0; spotlightIndexEveryOther < hallwaySpotlightPairs.Length; spotlightIndexEveryOther += 2)
        {
            while (hallwaySpotlightPairs[spotlightIndexEveryOther].intensity < maxBigSpotlightIntensity)
            {
                hallwaySpotlightPairs[spotlightIndexEveryOther].intensity += bigSpotlightFadeSpeed * Time.deltaTime;
                hallwaySpotlightPairs[spotlightIndexEveryOther + 1].intensity += frontSpotlightFadeSpeed * Time.deltaTime;
                yield return null;
            }
        }

    }
}
