using System.Collections;
using UnityEngine;

public class ScientistSpotlight : MonoBehaviour
{
    [SerializeField] private Light spotlight;
    [SerializeField] private float maxSpotlightIntensity = 110f;
    [SerializeField] private bool spotlightEnabled = false;
    [SerializeField] private float spotlightDelay = 1f;

    private void Start()
    {
        //fade in the spotlight
        StartCoroutine(nameof(EnableSpotlight));
    }

    private IEnumerator EnableSpotlight()
    {
        yield return new WaitForSeconds(spotlightDelay);
        spotlightEnabled = true;
    }

    private void Update()
    {
        if (spotlightEnabled && spotlight != null)
        {
            spotlight.intensity = Mathf.Lerp(spotlight.intensity, maxSpotlightIntensity, Time.deltaTime);
        }
    }
}
