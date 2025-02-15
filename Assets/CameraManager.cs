using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera playerCamera; // �������� ���� PlayerCamera ����� Inspector

    void Start()
    {
        // ���������, ���� �� ������
        if (playerCamera == null)
        {
            playerCamera = Camera.main; // ���� �������� ������
        }

        // �������� ������ ������ ������
        if (playerCamera != null)
        {
            Debug.Log(" �������� ������ ���������: " + playerCamera.name);
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(" ������ ��������� �� �������!");
        }

        Cursor.lockState = CursorLockMode.Locked; // ��������� ������
        Cursor.visible = false; // �������� ������
    }
}