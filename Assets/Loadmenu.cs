using UnityEngine;
using UnityEngine.SceneManagement;

public class Loadmenu : MonoBehaviour
{
    public void loadMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}

