using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _raycastDistance = 6f; // Попробуйте увеличить расстояние
    private Outline lastOutlineObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit; ;

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _raycastDistance, _layerMask))
            {
                // Проверяем наличие двери
                if (hit.collider.TryGetComponent(out OpenableObject openableObject))
                {
                    openableObject.OpenOrClose();
                }
                // Проверяем наличие аптечки
                else if (hit.collider.TryGetComponent(out Med med))
                {
                    med.Heal();
                }
            }
        }
    }
}