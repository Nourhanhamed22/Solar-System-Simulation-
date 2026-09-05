using UnityEngine;
using System.Collections;
using TMPro;

public class CameraFlyIn : MonoBehaviour
{
    [Header("Camera Positions")]
    public Transform outsidePosition;
    public Transform cloudPosition;
    public Transform liquidHPosition;
    public Transform metallicHPosition;
    public Transform corePosition;

    [Header("UI")]
    public TextMeshProUGUI layerNameText;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI layerInfoText;

    [Header("Objects to Hide")]
    public GameObject moonsParent;

    [Header("Settings")]
    public float flySpeed = 2f;

    private int currentLayer = 0;
    private bool isFlying = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isFlying)
        {
            if (currentLayer == 0 && cloudPosition != null)
                StartCoroutine(FlyTo(cloudPosition, 1,
                    "CLOUD TOPS",
                    "PRESS SPACE TO GO DEEPER",
                    "Temp: -145°C\nComposition: Ammonia & Ice\nThickness: ~50 km\nWinds up to 620 m/s"));

            else if (currentLayer == 1 && liquidHPosition != null)
                StartCoroutine(FlyTo(liquidHPosition, 2,
                    "LIQUID HYDROGEN",
                    "PRESS SPACE TO GO DEEPER",
                    "Temp: -110°C → +2000°C\nPressure: 2 million atm\nDepth: ~20,000 km\nBehaves like a liquid ocean"));

            else if (currentLayer == 2 && metallicHPosition != null)
                StartCoroutine(FlyTo(metallicHPosition, 3,
                    "METALLIC HYDROGEN",
                    "PRESS SPACE TO REACH THE CORE",
                    "Temp: +10,000°C\nPressure: 4 million atm\nDepth: ~40,000 km\nConducts electricity — generates Jupiter's magnetic field"));

            else if (currentLayer == 3 && corePosition != null)
                StartCoroutine(FlyTo(corePosition, 4,
                    "CORE — Rock & Ice",
                    "PRESS SPACE TO EXIT",
                    "Temp: +35,000°C\nPressure: 50 million atm\nSize: ~1.5x Earth\nSolid or semi-liquid rocky/icy core"));

            else if (currentLayer == 4 && outsidePosition != null)
                StartCoroutine(FlyTo(outsidePosition, 0,
                    "SURFACE VIEW",
                    "PRESS SPACE TO FLY INSIDE",
                    "Jupiter — Largest planet in the Solar System\nDiameter: 142,984 km\n318x Earth's mass"));
        }
    }

    IEnumerator FlyTo(Transform target, int nextLayer,
        string layerName, string instruction, string info)
    {
        if (target == null)
        {
            isFlying = false;
            yield break;
        }

        isFlying = true;

        if (moonsParent != null)
        {
            if (nextLayer >= 2)
                moonsParent.SetActive(false);
            else
                moonsParent.SetActive(true);
        }

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * flySpeed;
            transform.position = Vector3.Lerp(startPos, target.position, t);
            transform.rotation = Quaternion.Lerp(startRot, target.rotation, t);
            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
        currentLayer = nextLayer;
        isFlying = false;

        if (layerNameText != null) layerNameText.text = layerName;
        if (instructionText != null) instructionText.text = instruction;
        if (layerInfoText != null) layerInfoText.text = info;
    }
}