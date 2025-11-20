using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [Header("Параметри урону")]
    public int baseDamage = 25;
    public float attackRange = 2f;
    public float critChance = 0.3f;
    public float critMultiplier = 1.25f;

    [Header("Режим зброї")]
    public bool isThrown = false;
    private float thrownTimer = 0f;
    private float thrownDuration = 3f;
    private int originalBaseDamage;
    private bool hitboxActive = false;
    private bool isInHand = false;
    private bool hasRegisteredHitThisSwing = false;

    [Header("Ефекти ударів")]
    public GameObject critEffectPrefab;

    private Transform playerCamera;

    void Start()
    {
        playerCamera = Camera.main.transform;
        originalBaseDamage = baseDamage;

        if (!isInHand && !isThrown)
        {
            baseDamage = 0;
        }
    }

    void Update()
    {
        if (isThrown)
        {
            thrownTimer += Time.deltaTime;
            if (thrownTimer >= thrownDuration)
            {
                baseDamage = 0;
                isThrown = false;
            }
        }
        else if (!isInHand)
        {
            baseDamage = 0;
        }
    }

    public struct AttackResult
    {
        public bool didHit;
        public bool isCritical;
        public Vector3 hitPoint;
    }

    public AttackResult PerformAttackRaycast()
    {
        AttackResult result = new AttackResult();

        if (!isInHand || baseDamage <= 0)
        {
            baseDamage = 0;
            return result;
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
        {
            EnemyScript enemy = hit.collider.GetComponentInParent<EnemyScript>();
            if (enemy != null)
            {
                result.didHit = true;
                result.hitPoint = hit.point;

                bool isCrit = Random.value <= critChance;
                result.isCritical = isCrit;

                int finalDamage = isCrit
                    ? Mathf.RoundToInt(baseDamage * critMultiplier)
                    : baseDamage;

                enemy.TakeDamage(finalDamage);

                if (isCrit && critEffectPrefab)
                    Instantiate(critEffectPrefab, hit.point, Quaternion.identity);
            }
        }

        return result;
    }

    public void OnPickup()
    {
        isInHand = true;
        isThrown = false;
        thrownTimer = 0f;
        baseDamage = originalBaseDamage;
    }

    public void OnThrow()
    {
        isInHand = false;
        isThrown = true;
        thrownTimer = 0f;
        baseDamage = 0;
    }

    public void SetDamage(int damage)
    {
        baseDamage = damage;
    }

    public void SetThrownMode(bool thrown)
    {
        isThrown = thrown;
        thrownTimer = 0f;

        if (thrown)
        {
            baseDamage = 0;
        }
        else
        {
            if (isInHand)
            {
                baseDamage = originalBaseDamage;
            }
            else
            {
                baseDamage = 0;
            }
        }
    }

    public void SetActiveHitbox(bool active)
    {
        hitboxActive = active;
        if (active)
        {
            hasRegisteredHitThisSwing = false;
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = active;
            if (active)
                col.isTrigger = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isThrown)
        {
            baseDamage = 0;
            return;
        }

        EnemyScript enemy = collision.collider.GetComponentInParent<EnemyScript>();
        if (enemy != null && baseDamage > 0 && !isInHand)
        {
            bool isCrit = Random.value <= critChance;
            int finalDamage = isCrit
                ? Mathf.RoundToInt(baseDamage * critMultiplier)
                : baseDamage;

            enemy.TakeDamage(finalDamage);

            if (isCrit && critEffectPrefab)
            {
                Vector3 hitPoint = collision.contacts.Length > 0
                    ? collision.contacts[0].point
                    : collision.transform.position;
                Instantiate(critEffectPrefab, hitPoint, Quaternion.identity);
            }
        }

        // Після будь-якого зіткнення скидаємо стан кидка, щоб зброя не наносила урон на землі
        baseDamage = 0;
        isThrown = false;
    }

    void OnTriggerEnter(Collider other)
    {
        EnemyScript enemy = other.GetComponentInParent<EnemyScript>();
        TryDealDamage(enemy, other.ClosestPoint(transform.position));
    }

    public void TryDealDamage(EnemyScript enemy, Vector3 hitPoint)
    {
        if (!hitboxActive || !isInHand || baseDamage <= 0 || hasRegisteredHitThisSwing)
        {
            if (!isInHand)
            {
                baseDamage = 0;
            }
            return;
        }

        if (enemy == null)
            return;

        bool isCrit = Random.value <= critChance;
        int finalDamage = isCrit
            ? Mathf.RoundToInt(baseDamage * critMultiplier)
            : baseDamage;

        enemy.TakeDamage(finalDamage);

        if (isCrit && critEffectPrefab)
        {
            Instantiate(critEffectPrefab, hitPoint, Quaternion.identity);
        }

        hasRegisteredHitThisSwing = true;
        hitboxActive = false;
    }

    private bool IsWeaponInHand()
    {
        return isInHand;
    }
}

