using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public static int playerhealth;
    public static bool gameOver;
    public TextMeshProUGUI playerhealtText;
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
            SceneManager.LoadScene("SampleScene");
        }
    }


    public static void Damage(int damageCount)
    {
        playerhealth -= damageCount;

        if (playerhealth <= 0)
            gameOver = true;
    }
}
