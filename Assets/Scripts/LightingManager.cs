using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{

    public List<Light> headlights;

    public virtual void ToggleHeadLights()
    {
        foreach (Light light in headlights)
        {
            light.intensity = light.intensity == 0 ? 75f : 0f;
        }
    }
}
