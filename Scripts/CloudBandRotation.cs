using UnityEngine;

public class CloudBandRotation : MonoBehaviour
{
    [Header("Band Settings")]
    public float rotationSpeed = 4f;
    public float axisTiltDegrees = 2f;
    public bool reverseDirection = false;

    private Vector3 rotAxis;

    void Start()
    {
        rotAxis = Quaternion.Euler(axisTiltDegrees, 0, 0)
                  * Vector3.up;
        if (reverseDirection)
            rotationSpeed = -rotationSpeed;
    }

    void Update()
    {
        transform.Rotate(
            rotAxis * rotationSpeed * Time.deltaTime,
            Space.World
        );
    }
}