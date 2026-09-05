using UnityEngine;

[CreateAssetMenu(fileName = "PlanetData",
menuName = "SolarSystem/PlanetData")]
public class PlanetData : ScriptableObject
{
    [Header("Identity")]
    public string planetName;
    public Sprite planetIcon;

    [Header("Facts")]
    [TextArea(3, 6)] public string description;
    public string diameter;
    public string distanceFromSun;
    public string orbitalPeriod;
    public string rotationPeriod;
    public string numberOfMoons;
    public string surfaceTemperature;
    public string gravity;
    public string atmosphere;
    public string funFact;

    [Header("Audio")]
    public AudioClip planetAmbientSound;
    public float soundVolume = 0.6f;
}