using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;
    public TextMeshProUGUI playerhealtText; // Добавляем ссылку на объект PlayerHealth
    public GameObject dot; // Добавляем ссылку на объект dot
    public GameObject pause; // Добавляем ссылку на объект GameManager
    public GameObject settingsMenu; // Добавляем ссылку на объект SettingsMenu

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        // Проверяем активность settingsMenu и pauseGameMenu
        if (settingsMenu.activeSelf || pauseGameMenu.activeSelf)
        {
            playerhealtText.gameObject.SetActive(false); // Отключаем PlayerHealth
            dot.SetActive(false); // Отключаем dot
        }
        else
        {
            playerhealtText.gameObject.SetActive(true); // Включаем PlayerHealth
            dot.SetActive(true); // Включаем dot
        }

        // Управляем состоянием курсора
        if (settingsMenu.activeSelf || pauseGameMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; // Останавливаем время, если settingsMenu или pauseGameMenu активен
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // Возобновляем время, если settingsMenu и pauseGameMenu не активны
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        if (settingsMenu.activeSelf)
        {
            return; // Если settingsMenu активен, не активируем pauseGameMenu
        }

        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ShowSettingsMenu()
    {
        pauseGameMenu.SetActive(false); // Скрываем pauseGameMenu
        settingsMenu.SetActive(true); // Показываем SettingsMenu
        Time.timeScale = 0f; // Останавливаем время
        Cursor.lockState = CursorLockMode.None; // Разблокируем курсор
        Cursor.visible = true;
    }

    public void HideSettingsMenu()
    {
        settingsMenu.SetActive(false); // Скрываем SettingsMenu
        pauseGameMenu.SetActive(true); // Показываем pauseGameMenu
        Time.timeScale = 0f; // Останавливаем время
        Cursor.lockState = CursorLockMode.None; // Разблокируем курсор
        Cursor.visible = true;
    }
}


