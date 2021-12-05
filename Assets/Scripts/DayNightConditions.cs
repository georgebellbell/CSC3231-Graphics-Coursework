using UnityEngine;

// Asset for determining how the directional light and its colour will change throughout the day, as shown in DayNightController
[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Default", menuName = "Scriptables/Lighting Default", order = 1)]
public class DayNightConditions : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;

}
