using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RingGenerator : MonoBehaviour
{
    [Header("Ring Settings")]
    public float innerRadius = 1.4f;
    public float outerRadius = 2.8f;
    public int segments = 128;

    [Header("Material")]
    public Material ringMaterial;

    void Start()
    {
        GenerateRing();
    }

    void GenerateRing()
    {
        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[segments * 2];
        int[] tris = new int[segments * 6];
        Vector2[] uvs = new Vector2[segments * 2];

        for (int i = 0; i < segments; i++)
        {
            float angle = (i / (float)segments)
                          * Mathf.PI * 2f;
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            // Inner ring vertex
            verts[i * 2] = new Vector3(
                cos * innerRadius,
                0,
                sin * innerRadius);

            // Outer ring vertex
            verts[i * 2 + 1] = new Vector3(
                cos * outerRadius,
                0,
                sin * outerRadius);

            // UVs
            uvs[i * 2] = new Vector2(0, i / (float)segments);
            uvs[i * 2 + 1] = new Vector2(1, i / (float)segments);

            // Triangles
            int next = (i + 1) % segments;
            int idx = i * 6;

            tris[idx] = i * 2;
            tris[idx + 1] = next * 2;
            tris[idx + 2] = i * 2 + 1;

            tris[idx + 3] = next * 2;
            tris[idx + 4] = next * 2 + 1;
            tris[idx + 5] = i * 2 + 1;
        }

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        // Assign material if provided
        if (ringMaterial != null)
            GetComponent<MeshRenderer>().material = ringMaterial;
    }
}