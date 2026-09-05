using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public static string CurrentPlanet = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadPlanetSimulation(string planetName)
    {
        CurrentPlanet = planetName;
        SceneManager.LoadScene(planetName + "_Simulation");
    }

    public void ReturnToSolarSystem()
    {
        SceneManager.LoadScene("SolarSystem");
    }
}