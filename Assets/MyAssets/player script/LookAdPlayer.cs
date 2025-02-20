using UnityEngine;

public class LookAdPlayer : MonoBehaviour
{

    public Transform camera;

    void LateUpdate()
    {
        transform.LookAt(camera);
    }
}
