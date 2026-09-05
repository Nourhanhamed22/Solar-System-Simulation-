using UnityEngine;
using UnityEngine.UI;

public class BackButtonHandler : MonoBehaviour
{
    void Start()
    {
        // بيحاول يجيب الزرار من نفس الكائن
        Button btn = GetComponent<Button>();

        if (btn != null)
        {
            btn.onClick.AddListener(OnBackButtonClicked);
            Debug.Log("Back Button linked successfully on: " + gameObject.name);
        }
        else
        {
            // لو السكريبت في مكان غلط هيطبع لك التحذير ده
            Debug.LogError("ERROR: BackButtonHandler is on [" + gameObject.name + "] but there is no Button component here!");
        }
    }

    void OnBackButtonClicked()
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.ReturnToSolarSystem();
        }
        else
        {
            Debug.LogError("SceneLoader.Instance is NULL! Make sure SceneLoader is in your first scene.");
        }
    }
}