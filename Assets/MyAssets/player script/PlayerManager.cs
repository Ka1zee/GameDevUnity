using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Security.Cryptography.X509Certificates;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public static int playerhealth;
    public static bool gameOver;
    public TextMeshProUGUI playerhealtText;
    public GameObject completeUI;
    public void Complete()
    {
        completeUI.SetActive(true);
    }
    void Start()
    {
        playerhealth = 100;
        gameOver = false;
    }


    void Update()
    {
        playerhealtText.text = "" + playerhealth;

        if (gameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(2);
        }
    }


    public static void Damage(int damageCount)
    {
        playerhealth -= damageCount;

        if (playerhealth <= 0)
            gameOver = true;
    }
}
