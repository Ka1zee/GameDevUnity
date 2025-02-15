using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _checkGround;
    [SerializeField] private LayerMask _groundMask;

    [Header("Settings")]
    [SerializeField] private float _checkRadiusSphere = 0.2f;
    [SerializeField] private float _gravity = -14f;
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _speedRun = 7f;
    [SerializeField] private float _jumpHeight = 1f;

    [Range(1, 100)]
    [SerializeField] private float _sensitivity = 50f;

    private float rotationX;
    private bool isGrounded;
    private Vector3 velocity;
    private Vector3 move;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Переконаємося, що _characterController не null
        if (_characterController == null)
        {
            _characterController = GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

        // Обмежуємо кут огляду по вертикалі (-90, 90)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, 0f, 135f);

        // Камеру нахиляємо тільки по X
        _cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Повертаємо персонажа навколо осі Y
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move *= Input.GetKey(KeyCode.LeftShift) ? _speedRun : _speed;

        if (_characterController != null)
        {
            _characterController.Move(move * Time.deltaTime);
        }
    }
}
