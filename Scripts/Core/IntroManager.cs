using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    [Header("UI References")]
    public VideoPlayer videoPlayer;
    public GameObject videoScreen;
    public GameObject blackScreen;
    public GameObject menuUI;

    [Header("Settings")]
    public string nextSceneName = "SolarSystem";
    public float fadeSpeed = 1.5f;

    private bool _started = false;

    void Start()
    {
        // الحالة الابتدائية
        ShowMenu();
    }

    void ShowMenu()
    {
        if (menuUI != null)
            menuUI.SetActive(true);
        if (videoScreen != null)
            videoScreen.SetActive(false);
        if (blackScreen != null)
        {
            blackScreen.SetActive(true);
            Image img = blackScreen
                .GetComponent<Image>();
            if (img != null)
            {
                Color c = img.color;
                c.a = 0f;
                img.color = c;
            }
        }
    }

    // بتتكال من Start Button
    public void StartGame()
    {
        Debug.Log("START GAME CALLED ✓");
        if (_started) return;
        _started = true;
        StartCoroutine(RunIntro());
    }

    IEnumerator RunIntro()
    {
        // اخفي الـ Menu
        if (menuUI != null)
            menuUI.SetActive(false);

        // اظهر الـ Video Screen
        if (videoScreen != null)
            videoScreen.SetActive(true);

        // شغل الفيديو
        if (videoPlayer != null)
        {
            videoPlayer.Play();

            // استنى يبدأ
            float timer = 0f;
            while (!videoPlayer.isPlaying
                   && timer < 5f)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // استنى يخلص
            while (videoPlayer.isPlaying)
            {
                // لو الفيديو خلص
                if (videoPlayer.time >=
                    (double)videoPlayer.clip.length - 0.1f)
                    break;
                yield return null;
            }
        }

        // اخفي الفيديو
        if (videoScreen != null)
            videoScreen.SetActive(false);

        // Fade to Black
        yield return StartCoroutine(FadeToBlack());

        // انتقل للـ Solar System
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeToBlack()
    {
        if (blackScreen == null) yield break;

        blackScreen.SetActive(true);
        Image img = blackScreen
            .GetComponent<Image>();
        if (img == null) yield break;

        float elapsed = 0f;
        Color c = img.color;
        c.a = 0f;
        img.color = c;

        while (elapsed < fadeSpeed)
        {
            if (img == null) yield break;
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(
                elapsed / fadeSpeed);
            img.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
    }

    // Skip بـ Escape
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)
            && _started)
        {
            StopAllCoroutines();
            SceneManager.LoadScene(nextSceneName);
        }
    }
}