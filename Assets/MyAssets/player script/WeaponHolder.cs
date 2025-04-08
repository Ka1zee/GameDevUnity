using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public bool HasWeapon { get; private set; }

    [Header("Налаштування")]
    public Transform weaponHolder;
    public float pickupRange = 3f;
    public float throwForce = 5f;
    public LayerMask weaponLayer; // Додайте шар для зброї в інспекторі

    [Header("Позиція зброї")]
    public Vector3 holdPosition = new Vector3(0.1f, -0.1f, 0.2f);
    public Vector3 holdRotation = new Vector3(0, 0, 0);

    private GameObject currentWeapon;
    private Collider weaponCollider;
    private Rigidbody weaponRigidbody;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !HasWeapon)
        {
            TryPickup();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && HasWeapon)
        {
            DropWeapon();
        }
    }

    void LateUpdate()
    {
        if (HasWeapon && currentWeapon != null)
        {
            // Примусово оновлюємо позицію та ротацію зброї в руці
            currentWeapon.transform.localPosition = holdPosition;
            currentWeapon.transform.localRotation = Quaternion.Euler(holdRotation);
        }
    }

    void TryPickup()
    {
        if (weaponHolder == null)
        {
            Debug.LogError("weaponHolder не встановлений! Переконайся, що він існує у персонажі.");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
            out hit, pickupRange, weaponLayer))
        {
            if (hit.collider.CompareTag("Weapon"))
            {
                GrabWeapon(hit.collider.gameObject);
            }
        }
    }

    void GrabWeapon(GameObject weapon)
    {
        currentWeapon = weapon;
        weaponCollider = currentWeapon.GetComponent<Collider>();
        weaponRigidbody = currentWeapon.GetComponent<Rigidbody>();

        if (weaponRigidbody == null || weaponCollider == null)
        {
            Debug.LogError("Об'єкт зброї не має Rigidbody або Collider!");
            return;
        }

        // Вимкнути фізику
        weaponRigidbody.isKinematic = true;
        weaponCollider.isTrigger = true;

        // Приєднати до руки
        currentWeapon.transform.SetParent(weaponHolder);
        currentWeapon.transform.localPosition = holdPosition;
        currentWeapon.transform.localRotation = Quaternion.Euler(holdRotation);

        HasWeapon = true;
    }

    void DropWeapon()
    {
        if (!HasWeapon || currentWeapon == null) return;

        // Від'єднати від руки
        currentWeapon.transform.SetParent(null);

        // Увімкнути фізику
        weaponCollider.isTrigger = false;
        weaponRigidbody.isKinematic = false;
        weaponRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        weaponRigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        // Додати силу кидка
        weaponRigidbody.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
        weaponRigidbody.AddTorque(new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(-0.5f, 0.5f),
            Random.Range(-0.5f, 0.5f)) * 2f, ForceMode.Impulse);

        // Скинути посилання
        currentWeapon = null;
        weaponCollider = null;
        weaponRigidbody = null;
        HasWeapon = false;
    }

    public void EnableHitbox()
    {
        if (weaponCollider != null)
            weaponCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        if (weaponCollider != null)
            weaponCollider.enabled = false;
    }
}
