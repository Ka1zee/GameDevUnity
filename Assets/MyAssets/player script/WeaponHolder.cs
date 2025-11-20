using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public bool HasWeapon { get; private set; }

    [Header("Налаштування")]
    public Transform weaponHolder;
    public float pickupRange = 3f;
    public float throwForce = 8f;
    public LayerMask weaponLayer;
    [Tooltip("Шар для зброї в руці (0 = автоматично береться з weaponHolder, щоб не йшла під текстури)")]
    public int weaponLayerInHand = 0;
    [Tooltip("Шар для зброї на землі (0 = використовується шар weaponHolder, щоб не йшла під текстури)")]
    public int weaponLayerOnGround = 0;

    [Header("Позиція зброї в руці")]
    public Vector3 holdPosition = new Vector3(0.1f, -0.1f, 0.5f);
    public Vector3 holdRotation = new Vector3(0, 0, 0);
    [Tooltip("Порядок рендерингу для зброї (більше = рендериться пізніше/поверх інших)")]
    public int weaponSortingOrder = 100;

    [Header("Thrown damage settings")]
    public float thrownDamageDuration = 3f;

    private GameObject currentWeapon;
    private Collider weaponCollider;
    private Rigidbody weaponRigidbody;
    private WeaponDamage weaponDamageComponent;
    private int originalWeaponLayer;
    private readonly List<Renderer> weaponRenderers = new List<Renderer>();
    private readonly List<int> originalSortingOrders = new List<int>();
    private GameObject droppedWeapon;

    // Тривалість активного вікна хітбоксу під час анімації атаки
    public float attackHitWindow = 0.25f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !HasWeapon)
            TryPickup();
        else if (Input.GetKeyDown(KeyCode.Q) && HasWeapon)
            DropWeapon();
    }

    void LateUpdate()
    {
        if (HasWeapon && currentWeapon != null)
        {
            currentWeapon.transform.localPosition = holdPosition;
            currentWeapon.transform.localRotation = Quaternion.Euler(holdRotation);
        }
    }



    bool IsLayerAllowed(int objLayer, bool hasLayerMask)
    {
        return !hasLayerMask || ((1 << objLayer) & weaponLayer.value) != 0;
    }

    void TryPickup()
    {
        if (weaponHolder == null)
        {
            Debug.LogError("weaponHolder не встановлений!");
            return;
        }

        // Шукаємо всі колайдери поруч
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange, weaponLayer);

        if (hits.Length == 0)
            return;

        // Шукаємо найближчу зброю
        Collider closest = null;
        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = hit;
            }
        }

        if (closest == null)
            return;

        GameObject weaponObj = closest.gameObject;

        // Перевіряємо чи можна підібрати
        if (!IsLayerAllowed(weaponObj.layer, weaponLayer.value != 0))
            return;

        GrabWeapon(weaponObj);
    }

    void GrabWeapon(GameObject weapon)
    {
        currentWeapon = weapon;

        weaponCollider = weapon.GetComponent<Collider>();
        weaponRigidbody = weapon.GetComponent<Rigidbody>();
        weaponDamageComponent = weapon.GetComponent<WeaponDamage>();

        if (weaponCollider == null || weaponRigidbody == null)
        {
            Debug.LogError("Зброя не має колайдера або rigidbody");
            return;
        }

        // Запам'ятовуємо оригінальний шар
        originalWeaponLayer = weapon.layer;

        // Ставимо шар зброї у руці
        SetWeaponLayerRecursive(weapon, weaponLayerInHand);

        // Виключаємо фізику
        weaponRigidbody.isKinematic = true;
        weaponRigidbody.useGravity = false;

        // Колайдер завжди тригер у руці
        weaponCollider.isTrigger = true;

        // Кріпимо до руки
        weapon.transform.SetParent(weaponHolder);
        weapon.transform.localPosition = holdPosition;
        weapon.transform.localRotation = Quaternion.Euler(holdRotation);

        // Оновлюємо стан
        if (weaponDamageComponent != null)
        {
            weaponDamageComponent.OnPickup();
            weaponDamageComponent.SetThrownMode(false);
            weaponDamageComponent.SetActiveHitbox(false);
        }

        HasWeapon = true;
    }


    public WeaponAttackResult PerformAttackRaycast()
    {
        WeaponAttackResult result = new WeaponAttackResult();

        if (!HasWeapon || currentWeapon == null)
            return result;

        if (weaponDamageComponent != null)
        {
            var damageResult = weaponDamageComponent.PerformAttackRaycast();
            result.didHit = damageResult.didHit;
            result.isCritical = damageResult.isCritical;
            result.hitPoint = damageResult.hitPoint;
            return result;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            result.didHit = true;
            result.hitPoint = hit.point;

            bool isCrit = Random.value <= 0.3f;
            result.isCritical = isCrit;

            float baseDamage = 10f;
            float damage = isCrit ? baseDamage * 1.25f : baseDamage;

            var enemy = hit.collider.GetComponentInParent<EnemyScript>();
            if (enemy != null)
                enemy.TakeDamage((int)damage);
        }
        else
        {
            result.didHit = false;
        }

        return result;
    }

    public void EnableHitbox()
    {
        if (!HasWeapon || currentWeapon == null) return;
        weaponDamageComponent?.SetActiveHitbox(true);
    }

    public void DisableHitbox()
    {
        if (weaponDamageComponent != null)
            weaponDamageComponent.SetActiveHitbox(false);
    }

    public void EnableHitboxForWindow()
    {
        EnableHitbox();
        CancelInvoke(nameof(DisableHitbox));
        Invoke(nameof(DisableHitbox), attackHitWindow);
    }

    void DropWeapon()
    {
        if (!HasWeapon || currentWeapon == null)
            return;

        droppedWeapon = currentWeapon;

        // Відв'язуємо зброю від руки
        currentWeapon.transform.SetParent(null);

        // Повертаємо фізику
        weaponRigidbody.isKinematic = false;
        weaponRigidbody.useGravity = true;

        // Колайдер більше НЕ trigger
        weaponCollider.isTrigger = false;
        weaponCollider.enabled = true;

        // Встановлюємо землю-шар
        SetWeaponLayerRecursive(currentWeapon, weaponLayerOnGround);

        // Відкидаємо
        weaponRigidbody.AddForce(Camera.main.transform.forward * throwForce, ForceMode.VelocityChange);
        weaponRigidbody.AddTorque(Random.insideUnitSphere * 3f, ForceMode.Impulse);

        // Налаштування damage
        if (weaponDamageComponent != null)
        {
            weaponDamageComponent.OnThrow();
            StartCoroutine(EnableThrownDamageCoroutine(weaponDamageComponent, thrownDamageDuration));
        }

        // Очищаємо стейт
        currentWeapon = null;
        weaponCollider = null;
        weaponRigidbody = null;
        weaponDamageComponent = null;

        HasWeapon = false;
    }


    void SetWeaponSortingOrder(GameObject obj, int sortingOrder)
    {
        if (obj == null) return;

        weaponRenderers.Clear();
        originalSortingOrders.Clear();

        CollectRenderersRecursive(obj);

        for (int i = 0; i < weaponRenderers.Count; i++)
        {
            if (weaponRenderers[i] != null)
            {
                weaponRenderers[i].sortingOrder = sortingOrder;
            }
        }
    }

    void CollectRenderersRecursive(GameObject obj)
    {
        if (obj == null) return;

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            weaponRenderers.Add(renderer);
            originalSortingOrders.Add(renderer.sortingOrder);
        }

        foreach (Transform child in obj.transform)
        {
            CollectRenderersRecursive(child.gameObject);
        }
    }

    void RestoreWeaponSortingOrder()
    {
        for (int i = 0; i < weaponRenderers.Count && i < originalSortingOrders.Count; i++)
        {
            if (weaponRenderers[i] != null)
            {
                weaponRenderers[i].sortingOrder = originalSortingOrders[i];
            }
        }
    }

    void SetWeaponLayerRecursive(GameObject obj, int layer)
    {
        if (obj == null) return;

        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetWeaponLayerRecursive(child.gameObject, layer);
        }
    }

    IEnumerator EnableThrownDamageCoroutine(WeaponDamage wd, float duration)
    {
        wd.SetThrownMode(true);
        yield return new WaitForSeconds(duration);
        wd.SetThrownMode(false);

        var col = wd.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
            col.enabled = true;
        }

        if (droppedWeapon != null)
        {
            int groundLayer = weaponLayerOnGround;
            if (groundLayer == 0 && weaponHolder != null)
            {
                groundLayer = weaponHolder.gameObject.layer;
            }
            SetWeaponLayerRecursive(droppedWeapon, groundLayer);
            SetWeaponSortingOrderForGround(droppedWeapon);

            var damage = droppedWeapon.GetComponent<WeaponDamage>();
            if (damage != null && droppedWeapon.transform.parent == null)
            {
                damage.SetDamage(0);
            }
        }
    }

    void SetWeaponSortingOrderForGround(GameObject obj)
    {
        if (obj == null) return;

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = weaponSortingOrder;
        }

        foreach (Transform child in obj.transform)
        {
            SetWeaponSortingOrderForGround(child.gameObject);
        }
    }
}

public struct WeaponAttackResult
{
    public bool didHit;
    public bool isCritical;
    public Vector3 hitPoint;
}
