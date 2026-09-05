using UnityEngine;
using UnityEngine.UI;

public class PlanetSizeComparison : MonoBehaviour
{
    [System.Serializable]
    public class PlanetInfo
    {
        public Transform planet;
        public Transform planetMesh;
        public float realScale;      // accurate size
        public float normalScale;    // game size
        public Vector3 comparisonPos; // side by side pos
        public Vector3 normalPos;    // original pos
    }

    public PlanetInfo[] planets;
    public Button toggleButton;
    private bool _comparisonMode = false;

    void Start()
    {
        if (toggleButton != null)
            toggleButton.onClick.AddListener(ToggleMode);
    }

    public void ToggleMode()
    {
        _comparisonMode = !_comparisonMode;

        foreach (var p in planets)
        {
            if (p.planet == null) continue;

            if (_comparisonMode)
            {
                // Move to side by side positions
                p.planet.position = p.comparisonPos;
                // Set accurate relative scale
                p.planetMesh.localScale =
                    Vector3.one * p.realScale;
            }
            else
            {
                // Return to normal
                p.planet.position = p.normalPos;
                p.planetMesh.localScale =
                    Vector3.one * p.normalScale;
            }
        }
    }
}