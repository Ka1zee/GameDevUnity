using UnityEngine;
using UnityEngine.SceneManagement; // Додаємо, щоб відстежувати перезапуск

public class PlaySoundOnStart : MonoBehaviour
{
    private AudioSource audioSource;
    private static bool hasPlayed = false; // Статична змінна зберігається між об'єктами

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Скидаємо hasPlayed, якщо це новий запуск сцени
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            hasPlayed = false;
        }
    }

    void Update()
    {
        if (NewMonoBehaviourScript.gameOver && audioSource != null && !hasPlayed)
        {
            audioSource.Play();
            hasPlayed = true; // Запобігає повторному програванню
        }
    }
}


