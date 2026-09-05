using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject startButton;
    public TextMeshProUGUI loadingText;
    private bool _loading = false;

    void Start()
    {
        if (loadingText != null)
            loadingText.gameObject
                .SetActive(false);
    }

    public void StartSimulation()
    {
        if (_loading) return;
        _loading = true;

        if (loadingText != null)
        {
            loadingText.gameObject
                .SetActive(true);
            loadingText.text =
                "Loading Solar System...";
        }

        SceneManager.LoadScene("SolarSystem");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}