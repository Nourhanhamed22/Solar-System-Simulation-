using UnityEngine;

public class CometOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform sunTransform;
    public float orbitSpeed = 0.8f;
    public float semiMajorAxis = 200f;
    public float semiMinorAxis = 80f;
    public float tilt = 30f;

    private float _angle = 0f;

    void Update()
    {
        _angle += orbitSpeed
            * Time.deltaTime
            * TimeController.TimeScale;

        float rad = _angle * Mathf.Deg2Rad;

        float x = semiMajorAxis * Mathf.Cos(rad);
        float z = semiMinorAxis * Mathf.Sin(rad);
        float y = z * Mathf.Sin(tilt * Mathf.Deg2Rad);

        transform.position = new Vector3(x, y, z);

        // Always point forward in direction of travel
        Vector3 nextPos = new Vector3(
            semiMajorAxis * Mathf.Cos(rad + 0.01f),
            0,
            semiMinorAxis * Mathf.Sin(rad + 0.01f));

        if (nextPos != transform.position)
            transform.LookAt(nextPos);
    }
}