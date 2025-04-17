using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public static int playerhealth;
    public static bool gameOver;

    public TextMeshProUGUI playerhealtText;
    public GameObject completeUI;
    public Image healthFillImage; // <- нове: це твій бар заповнення (Image зі стилем Filled)

    private int maxHealth = 100;

    public void Complete()
    {
        completeUI.SetActive(true);
    }

    void Start()
    {
        playerhealth = maxHealth;
        gameOver = false;
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();

        if (gameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(2);
        }
    }

    void UpdateUI()
    {
        playerhealtText.text = playerhealth.ToString();

        // Обчислюємо заповнення від 0 до 1
        float fillAmount = Mathf.Clamp01((float)playerhealth / maxHealth);
        healthFillImage.fillAmount = fillAmount;
    }

    public static void Damage(int damageCount)
    {
        playerhealth -= damageCount;

        if (playerhealth <= 0)
            gameOver = true;
    }
}
