using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Solar View")]
    public Vector3 defaultPosition = new Vector3(0, 80, -50);
    public Vector3 defaultRotation = new Vector3(55, 0, 0);

    [Header("Transition")]
    public float focusSpeed = 3f;
    public float rotateSpeed = 30f;

    [Header("Free Camera")]
    public float moveSpeed = 30f;
    public float mouseSensitivity = 2f;
    public float zoomSpeed = 20f;

    [Header("Focus Ring")]
    public GameObject focusRing;

    private Vector3 _targetPos;
    private Quaternion _targetRot;
    private Transform _focusTarget;
    private bool _orbiting = false;
    private bool _isFocused = false;
    private float _focusDistance = 8f; // ← added

    void Start()
    {
        ReturnToSolarView();
    }

    void Update()
    {
        if (_isFocused && _focusTarget != null)
        {
            // Recalculate EVERY frame to follow moving planet
            Vector3 desiredPos = _focusTarget.position +
                new Vector3(0, _focusDistance * 0.4f, -_focusDistance);

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPos,
                focusSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(
                    _focusTarget.position - transform.position),
                focusSpeed * Time.deltaTime);

            return; // skip everything else
        }

        // Free camera
        transform.position = Vector3.Lerp(
            transform.position, _targetPos,
            focusSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(
            transform.rotation, _targetRot,
            focusSpeed * Time.deltaTime);

        if (!_isFocused)
            HandleFreeCamera();
    }

    void HandleFreeCamera()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h * moveSpeed
                     + transform.forward * v * moveSpeed;
        _targetPos += move * Time.deltaTime;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
            _targetPos += transform.forward * scroll * zoomSpeed;

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.RotateAround(Vector3.zero, Vector3.up, mouseX * 2f);
            transform.RotateAround(Vector3.zero, transform.right, -mouseY * 2f);

            _targetPos = transform.position;
            _targetRot = transform.rotation;
        }
    }

    void HandleFocusedCamera()
    {
        if (Input.GetMouseButton(1) && _focusTarget != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.RotateAround(_focusTarget.position, Vector3.up, mouseX * 2f);
            transform.RotateAround(_focusTarget.position, transform.right, -mouseY * 2f);

            _targetPos = transform.position;
            _targetRot = transform.rotation;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
            _targetPos += transform.forward * scroll * zoomSpeed;
    }

    public void FocusOn(Transform target, float distance)
    {
        _focusTarget = target;
        _orbiting = true;
        _isFocused = true;
        _focusDistance = distance; // ← saves distance for Update()

        Vector3 offset = new Vector3(0, distance * 0.4f, -distance);
        _targetPos = target.position + offset;
        _targetRot = Quaternion.LookRotation(target.position - _targetPos);

        if (focusRing != null)
        {
            focusRing.SetActive(true);
            focusRing.transform.SetParent(target);
            focusRing.transform.localPosition = Vector3.zero;
        }
    }

    public void ReturnToSolarView()
    {
        _orbiting = false;
        _isFocused = false;
        _focusTarget = null;
        _targetPos = defaultPosition;
        _targetRot = Quaternion.Euler(defaultRotation);

        if (focusRing != null)
        {
            focusRing.SetActive(false);
            focusRing.transform.SetParent(null);
        }
    }
}