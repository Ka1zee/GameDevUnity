using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera playerCamera; // Перетащи сюда PlayerCamera через Inspector

    void Start()
    {
        // Проверяем, есть ли камера
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // Ищем основную камеру
        }

        // Включаем только камеру игрока
        if (playerCamera != null)
        {
            Debug.Log(" Включаем камеру персонажа: " + playerCamera.name);
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(" Камера персонажа НЕ найдена!");
        }

        Cursor.lockState = CursorLockMode.Locked; // Блокируем курсор
        Cursor.visible = false; // Скрываем курсор
    }
}