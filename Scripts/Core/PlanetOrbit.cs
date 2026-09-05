using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform sunTransform;
    public float orbitRadius = 10f;
    public float orbitSpeed = 1f;
    public float orbitTilt = 0f;
    public bool clockwise = false;

    [Header("Visual")]
    public LineRenderer orbitLine;
    public int orbitLineSegments = 128;

    private float _currentAngle;
    private bool _initialized = false;

    void Start()
    {
        _currentAngle = Random.Range(0f, 360f);
        _initialized = true;
        DrawOrbitPath();
    }

    void Update()
    {
        if (!_initialized) return;
        if (sunTransform == null) return;

        // Get time scale safely
        float timeScale = 1f;
        if (TimeController.TimeScale > 0)
            timeScale = TimeController.TimeScale;

        float direction = clockwise ? -1f : 1f;
        _currentAngle += direction
            * orbitSpeed
            * Time.deltaTime
            * timeScale;

        float rad = _currentAngle * Mathf.Deg2Rad;

        float x = sunTransform.position.x
                  + orbitRadius * Mathf.Cos(rad);
        float z = sunTransform.position.z
                  + orbitRadius * Mathf.Sin(rad);
        float y = sunTransform.position.y
                  + orbitRadius
                  * Mathf.Sin(rad)
                  * Mathf.Sin(orbitTilt
                  * Mathf.Deg2Rad);

        // Move the parent planet object
        transform.position = new Vector3(x, y, z);
        Debug.Log("Earth pos: " + transform.position);
    }

    void DrawOrbitPath()
    {
        if (orbitLine == null) return;
        if (sunTransform == null) return;

        orbitLine.positionCount =
            orbitLineSegments + 1;
        orbitLine.loop = true;

        for (int i = 0; i <= orbitLineSegments; i++)
        {
            float angle = (i / (float)orbitLineSegments)
                         * 2f * Mathf.PI;
            float x = sunTransform.position.x
                      + orbitRadius * Mathf.Cos(angle);
            float z = sunTransform.position.z
                      + orbitRadius * Mathf.Sin(angle);
            float y = sunTransform.position.y
                      + orbitRadius
                      * Mathf.Sin(angle)
                      * Mathf.Sin(orbitTilt
                      * Mathf.Deg2Rad);

            orbitLine.SetPosition(i,
                new Vector3(x, y, z));
        }
    }
}