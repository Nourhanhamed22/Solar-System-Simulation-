using UnityEngine;

public class PlanetClickHandler : MonoBehaviour
{
    [Header("Data")]
    public PlanetData data;
    public float focusDistance = 6f;

    private static Camera _cam;
    private bool _mouseOver = false;

    // Ignore UI in raycast
    private int _layerMask;

    void Start()
    {
        _cam = Camera.main;
        _layerMask = ~(1 <<
            LayerMask.NameToLayer("UI"));
    }

    void Update()
    {
        if (_cam == null) return;

        Ray ray = _cam.ScreenPointToRay(
            Input.mousePosition);
        RaycastHit hit;

        bool hitThis = Physics.Raycast(
            ray, out hit, 5000f, _layerMask)
            && hit.collider.gameObject
            == gameObject;

        if (hitThis && !_mouseOver)
        {
            _mouseOver = true;
            OnHoverEnter();
        }

        if (!hitThis && _mouseOver)
        {
            _mouseOver = false;
            OnHoverExit();
        }

        if (hitThis &&
            Input.GetMouseButtonDown(0))
        {
            OnPlanetClick();
        }
    }

    void OnHoverEnter()
    {
        if (data == null) return;
        if (UIManager.Instance == null) return;
        UIManager.Instance.ShowPlanetHoverLabel(
            data.planetName, transform.position);
        Debug.Log("Hover: " + data.planetName);
    }

    void OnHoverExit()
    {
        if (UIManager.Instance == null) return;
        UIManager.Instance.HidePlanetHoverLabel();
    }

    void OnPlanetClick()
    {
        if (data == null) return;
        if (SolarSystemManager.Instance == null)
            return;
        Debug.Log("Clicked: " + data.planetName);
        SolarSystemManager.Instance
            .FocusPlanet(this);
    }
}