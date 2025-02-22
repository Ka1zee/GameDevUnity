using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Завантажуємо нову сцену
        SceneManager.LoadScene(1);

        // Додаємо слухач для події завантаження сцени
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Exit()
    {
        Application.Quit();
    }

    // Метод для обробки події після завантаження сцени
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1) // Перевіряємо, чи це правильна сцена
        {
            // Шукаємо гравця у новій сцені за тегом
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                // Знаходимо AudioSource на об'єкті гравця
                AudioSource audioSource = player.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Play(); // Відтворюємо звук на гравцеві
                }
                else
                {
                    Debug.LogError("No AudioSource found on Player!");
                }
            }
            else
            {
                Debug.LogError("Player not found in the scene!");
            }
        }

        // Видаляємо слухача події, щоб він не спрацьовував знову
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
