using UnityEngine;

public class ClickTester : MonoBehaviour
{
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
        Debug.Log("ClickTester started");
    }

    void Update()
    {
        Ray ray = _cam.ScreenPointToRay(
            Input.mousePosition);
        RaycastHit hit;

        // Ignore UI layer
        int layerMask = ~(1 << LayerMask.NameToLayer("UI"));

        if (Physics.Raycast(ray, out hit, 5000f, layerMask))
        {
            Debug.Log("Ray hitting: "
                + hit.collider.gameObject.name
                + " distance: " + hit.distance);

            if (Input.GetMouseButtonDown(0))
                Debug.Log("CLICKED: "
                    + hit.collider.gameObject.name);
        }
        else
        {
            // Uncomment to debug
            Debug.Log("Ray hitting nothing");
        }
    }
}