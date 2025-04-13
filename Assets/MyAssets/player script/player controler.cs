using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Slider slider;
    public Text sensa; // Добавляем ссылку на текстовый элемент sensa
    private float rotationX;
    public float mouseSensitivity = 100f;
    private bool isGrounded;
    private Vector3 velocity;
    private Vector3 move;

    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("SensitivityPreference", 100f);
        slider.value = mouseSensitivity / 10;
        UpdateSensitivityText(); // Обновляем текстовое значение чувствительности
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
        PlayerPrefs.SetFloat("SensitivityPreference", mouseSensitivity);
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Обмежуємо кут огляду по вертикалі (-90, 90)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, 0f, 160f);

        // Камеру нахиляємо тільки по X
        _cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Повертаємо персонажа навколо осі Y
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Move()
    {
        // Використовуємо GetAxisRaw для миттєвої реакції (-1, 0, 1)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Вектор руху відносно камери (без вертикальної складової)
        Vector3 moveDirection = _cameraTransform.right * moveX + _cameraTransform.forward * moveZ;
        moveDirection.y = 0;

        // Нормалізація та швидкість
        if (moveDirection.magnitude > 0)
            moveDirection.Normalize();

        // Миттєвий рух без фізики
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? _speedRun : _speed;
        _characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    public void AdjustSpeed(float newSpeed)
    {
        mouseSensitivity = newSpeed * 10;
        UpdateSensitivityText(); // Обновляем текстовое значение чувствительности
    }

    private void UpdateSensitivityText()
    {
        if (sensa != null)
        {
            sensa.text = mouseSensitivity.ToString("F1"); // Форматируем значение до одного знака после запятой
        }
    }
}


