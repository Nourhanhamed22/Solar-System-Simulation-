using UnityEngine;
using System.Collections;

namespace AstronautPlayer
{
    public class AstronautPlayer : MonoBehaviour
    {
        private Animator _animator;
        private bool _isMoving;
        private bool _nearPlanet;

        public float speed = 8.0f;
        public float turnSpeed = 3.0f;

        [Header("Float / Bob Effect")]
        public float floatAmplitude = 0.5f;
        public float floatFrequency = 0.8f;

        [Header("Planet Interaction")]
        public float nearPlanetDistance = 15f;
        public GameObject nearPlanetButton;

        [Header("Details Mode")]
        public GameObject astronautMesh;
        public Camera mainCamera;
        public MonoBehaviour astronautCameraScript; // ← drag AstronautThirdPersonCamera here
        public CameraController solarCameraScript;  // ← drag CameraController here

        private Vector3 startPosition;
        private GameObject _closestPlanet;
        private bool _inDetailsMode = false;

        private string[] _planetNames = {
            "Earth", "Mars", "Jupiter", "Saturn",
            "Venus", "Mercury", "Uranus", "Neptune"
        };

        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            startPosition = transform.position;

            if (nearPlanetButton != null)
                nearPlanetButton.SetActive(false);
        }

        void Update()
        {
            if (_inDetailsMode)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    ExitDetailsMode();
                return;
            }

            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            float upDown = 0f;

            if (Input.GetKey(KeyCode.Q)) upDown = 1f;
            if (Input.GetKey(KeyCode.E)) upDown = -1f;

            _isMoving = vertical != 0f || horizontal != 0f || upDown != 0f;

            UpdateAnimations();
            ApplyFloatBob();
            CheckNearPlanet();

            if (Input.GetKeyDown(KeyCode.Return) && _closestPlanet != null)
            {
                EnterDetailsMode();
                return;
            }

            if (_isMoving)
            {
                Vector3 move = (transform.forward * vertical)
                             + (transform.right * horizontal)
                             + (Vector3.up * upDown);

                transform.position += move * speed * Time.deltaTime;

                Vector3 horizontalMove = new Vector3(move.x, 0, move.z);
                if (horizontalMove != Vector3.zero)
                {
                    Quaternion targetRot =
                        Quaternion.LookRotation(horizontalMove);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRot,
                        turnSpeed * Time.deltaTime);
                }
            }
        }

        void EnterDetailsMode()
        {
            _inDetailsMode = true;

            // Hide all renderers
            Renderer[] renderers =
                GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in renderers)
                r.enabled = false;

            if (astronautMesh != null)
                astronautMesh.SetActive(false);

            if (nearPlanetButton != null)
                nearPlanetButton.SetActive(false);

            // Switch cameras using direct references
            if (astronautCameraScript != null)
                astronautCameraScript.enabled = false;

            if (solarCameraScript != null)
                solarCameraScript.enabled = true;

            // Open planet details
            PlanetClickHandler handler =
                _closestPlanet.GetComponent<PlanetClickHandler>();

            if (handler != null &&
                SolarSystemManager.Instance != null)
                SolarSystemManager.Instance.FocusPlanet(handler);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void ExitDetailsMode()
        {
            _inDetailsMode = false;

            // Show all renderers
            Renderer[] renderers =
                GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in renderers)
                r.enabled = true;

            if (astronautMesh != null)
                astronautMesh.SetActive(true);

            // Switch cameras back
            if (astronautCameraScript != null)
                astronautCameraScript.enabled = true;

            if (solarCameraScript != null)
                solarCameraScript.enabled = false;

            if (SolarSystemManager.Instance != null)
                SolarSystemManager.Instance.Unfocus();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void ApplyFloatBob()
        {
            if (!_isMoving)
            {
                float newY = startPosition.y
                    + Mathf.Sin(Time.time * floatFrequency)
                    * floatAmplitude;

                transform.position = new Vector3(
                    transform.position.x,
                    newY,
                    transform.position.z);
            }
            else
            {
                startPosition = new Vector3(
                    startPosition.x,
                    transform.position.y,
                    startPosition.z);
            }
        }

        void CheckNearPlanet()
        {
            _closestPlanet = null;
            float closestDist = nearPlanetDistance;

            foreach (string pName in _planetNames)
            {
                GameObject planet = GameObject.Find(pName);
                if (planet == null) continue;

                float dist = Vector3.Distance(
                    transform.position,
                    planet.transform.position);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    _closestPlanet = planet;
                }
            }

            _nearPlanet = _closestPlanet != null;

            if (nearPlanetButton != null)
                nearPlanetButton.SetActive(_nearPlanet);
        }

        public void OnNearPlanetButtonClick()
        {
            if (_closestPlanet == null) return;

            PlanetClickHandler handler =
                _closestPlanet.GetComponent<PlanetClickHandler>();

            if (handler != null &&
                SolarSystemManager.Instance != null)
                SolarSystemManager.Instance.FocusPlanet(handler);
        }

        void UpdateAnimations()
        {
            if (_animator == null) return;

            float moveSpeed = _isMoving ? 1f : 0f;

            try { _animator.SetFloat("Speed", moveSpeed); }
            catch { }

            try { _animator.SetBool("IsMoving", _isMoving); }
            catch { }

            if (_nearPlanet && !_isMoving)
            {
                try { _animator.speed = Mathf.Sin(Time.time * 3f) * 0.5f + 1.5f; }
                catch { }
            }
            else
            {
                try { _animator.speed = 1f; }
                catch { }
            }
        }
    }
}