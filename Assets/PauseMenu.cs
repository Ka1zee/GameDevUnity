using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;
    public TextMeshProUGUI playerhealtText;
    public GameObject dot;
    public GameObject settingsMenu;

    // Додане посилання на об'єкт PlayerHealth
    public GameObject playerHealth;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsMenu.activeSelf)
            {
                HideSettingsMenu();
            }
            else if (PauseGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        // Відключаємо HUD (здоров'я, приціл та PlayerHealth), якщо відкрите будь-яке меню
        bool anyMenuActive = settingsMenu.activeSelf || pauseGameMenu.activeSelf;
        playerhealtText.gameObject.SetActive(!anyMenuActive);
        dot.SetActive(!anyMenuActive);

        // Відключаємо PlayerHealth
        playerHealth.SetActive(!anyMenuActive);

        if (anyMenuActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
    }

    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ShowSettingsMenu()
    {
        pauseGameMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void HideSettingsMenu()
    {
        settingsMenu.SetActive(false);

        if (PauseGame)
        {
            pauseGameMenu.SetActive(true);
        }
    }
}
