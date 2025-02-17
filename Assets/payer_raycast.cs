using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float _raycastDistance = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _raycastDistance, layerMask))
            {
                Debug.Log("ќб'Їкт знайдено: " + hit.collider.name);

                OpenableObject openable = hit.collider.GetComponent<OpenableObject>();
                if (openable != null)
                {
                    openable.OpenOrClose();
                }
            }
        }
    }
}
