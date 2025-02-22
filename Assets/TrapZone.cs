using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrapEffect : MonoBehaviour
{
    public Image whiteScreen; // Ссылка на белый экран
    public float fadeDuration = 0.5f; // Длительность эффекта
    private AudioSource audioSource; // Аудио источник

    private void Start()
    {
        // Получаем компонент AudioSource (он должен быть на этом же объекте)
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Проверяем, игрок ли это
        {
            StartCoroutine(FlashWhiteScreen());

            // Включаем звук, если он есть и не играет
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    IEnumerator FlashWhiteScreen()
    {
        // Делаем экран полностью белым
        whiteScreen.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(fadeDuration);

        // Плавно уменьшаем прозрачность (fade out)
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            whiteScreen.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}
