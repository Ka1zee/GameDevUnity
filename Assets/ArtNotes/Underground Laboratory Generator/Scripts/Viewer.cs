using UnityEngine;

namespace ArtNotes.UndergroundLaboratoryGenerator
{
    public class Viewer : MonoBehaviour
    {
        [TextArea] public string Description = "Use on any GameObject with a Camera. WASD - move, Space/LeftCtrl - up/down, Mouse - look around, Shift - run";
        public float WalkSpeed = 2f, RunSpeed = 6f, Sensitivity = 2f;
        public bool HideCursor = true;

        private Camera cam;
        private float rotationX = 0f, rotationY = 0f;
        private float speed;

        private void Start()
        {
            cam = Camera.main;
            if (cam == null)
            {
                cam = FindObjectOfType<Camera>(); // Find any available camera
            }

            if (cam == null)
            {
                Debug.LogError("Viewer: No Camera found! Make sure there is a Camera in the scene.");
                enabled = false;
                return;
            }

            if (HideCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Update()
        {
            // Mouse look
            rotationX += Input.GetAxis("Mouse X") * Sensitivity;
            rotationY -= Input.GetAxis("Mouse Y") * Sensitivity;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Prevent full flips

            cam.transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);

            // Movement
            speed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;
            Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

            // Vertical movement
            if (Input.GetKey(KeyCode.Space)) move += transform.up;
            if (Input.GetKey(KeyCode.LeftControl)) move -= transform.up;

            transform.position += move * speed * Time.deltaTime;
        }
    }
}