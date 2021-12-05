using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [SerializeField] Light directionalLight;
    [SerializeField] DayNightConditions defaultConditions;

    [SerializeField, Range(0, 24)] float time;

    bool torchesOn;

    Light[] lights;

    private void Awake()
    {
        lights = FindObjectsOfType<Light>();
        ToggleTorches(false);

        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        
    }
    private void Update()
    {
        if (defaultConditions != null)
        {
            // progresses the scene through the day
            time += Time.deltaTime;
            time %= 24;
            UpdateLighting(time / 24f);

            // if it is night, i.e. darkness, the torches will be turned on. Otherwise they will be turned off
            if (ItsNight())
            {
                if (!torchesOn)
                {
                    ToggleTorches(true);
                    torchesOn = true;
                }
            }
            else
            {
                if (torchesOn)
                {
                    ToggleTorches(false);
                    torchesOn = false;
                }
            }
        }
    }

    void ToggleTorches(bool active)
    {
        foreach (Light light in lights)
        {
            if (light.gameObject.CompareTag("Torch"))
            {
                light.enabled = active;
            }

        }
    }

    // returns true if time is less than 6 and greater than 17, the time for when it is dark
    bool ItsNight()
    {
        return (time <= 6 || time >= 17);
    }  

    // takes the percentage through the day the scene is and makes the appropriate adjustments using the gradients from the instance of DayNightController
    void UpdateLighting(float timeThroughDayPercent)
    {
        RenderSettings.ambientLight = defaultConditions.AmbientColor.Evaluate(timeThroughDayPercent);

        if (directionalLight != null)
        {
            directionalLight.color = defaultConditions.DirectionalColor.Evaluate(timeThroughDayPercent);

            Vector3 newRotation = new Vector3((timeThroughDayPercent * 360f) - 90f, 170f, 0);
            directionalLight.transform.localRotation = Quaternion.Euler(newRotation);
        }
    }
}
