using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrapEffect : MonoBehaviour
{
    public Image whiteScreen; // Посилання на білий екран
    public float fadeDuration = 0.5f; // Тривалість ефекту

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Перевіряємо, чи це гравець
        {
            StartCoroutine(FlashWhiteScreen());
        }
    }

    IEnumerator FlashWhiteScreen()
    {
        // Робимо екран повністю білим
        whiteScreen.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(fadeDuration);

        // Плавно зменшуємо прозорість (fade out)
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            whiteScreen.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}

