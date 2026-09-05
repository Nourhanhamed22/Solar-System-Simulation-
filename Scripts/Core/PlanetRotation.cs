using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 10f;
    public float axialTilt = 0f;

    [Header("Cloud Layer")]
    public Transform cloudMesh;
    public float cloudRotationOffset = 1.5f;

    void Start()
    {
        Transform meshChild = transform.Find("PlanetMesh");
        if (meshChild != null)
            meshChild.localRotation = Quaternion.Euler(axialTilt, 0f, 0f);
    }

    void Update()
    {
        Transform meshChild = transform.Find("PlanetMesh");
        if (meshChild != null)
            meshChild.Rotate(Vector3.up, rotationSpeed
                * Time.deltaTime
                * TimeController.TimeScale);

        if (cloudMesh != null)
            cloudMesh.Rotate(Vector3.up,
            (rotationSpeed + cloudRotationOffset) * Time.deltaTime);
    }
}