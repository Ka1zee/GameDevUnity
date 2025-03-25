using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame;
    public GameObject pauseGameMenu;
    public TextMeshProUGUI playerhealtText;
    public GameObject dot;
    public GameObject settingsMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // якщо settingsMenu в≥дкритий Ч закриваЇмо його
            if (settingsMenu.activeSelf)
            {
                HideSettingsMenu();
            }
            // якщо гра на пауз≥, але settingsMenu не в≥дкритий Ч продовжуЇмо гру
            else if (PauseGame)
            {
                Resume();
            }
            // якщо гра не на пауз≥ Ч ставимо на паузу
            else
            {
                Pause();
            }
        }

        // ¬≥дключаЇмо HUD (здоров'€ та приц≥л), €кщо в≥дкрите будь-€ке меню
        bool anyMenuActive = settingsMenu.activeSelf || pauseGameMenu.activeSelf;
        playerhealtText.gameObject.SetActive(!anyMenuActive);
        dot.SetActive(!anyMenuActive);

        //  еруванн€ курсором та часом
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

        // якщо гра на пауз≥ Ч показуЇмо головне меню паузи
        if (PauseGame)
        {
            pauseGameMenu.SetActive(true);
        }
        // якщо гра не на пауз≥ Ч просто закриваЇмо settingsMenu (Resume вже оброблюЇ курсор ≥ час)
    }
}