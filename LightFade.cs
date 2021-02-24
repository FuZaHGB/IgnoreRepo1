using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightFade : MonoBehaviour
{
    public float minIntensity;
    public float maxIntensity;
    public float delay;
    public bool startAtMin;

    private Light barLight;

    private float timeElapsed;

    private void Awake()
    {
        barLight = this.GetComponentInChildren<Light>();

        if (barLight != null)
        {
            barLight.intensity = startAtMin ? minIntensity : maxIntensity;
            barLight.color = UnityEngine.Color.red;
        }
    }

    private void Update()
    {
        if (barLight != null)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= delay)
            {
                timeElapsed = 0;
                ToggleLight();
            }
        }
    }

    private void ToggleLight()
    {
        if (barLight != null)
        {
            if (barLight.intensity == minIntensity)
            {
                barLight.intensity = maxIntensity;
            }

            else if (barLight.intensity == maxIntensity)
            {
                barLight.intensity = minIntensity;
            }
        }
    }
}
